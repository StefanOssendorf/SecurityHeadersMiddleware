using System;
using System.Text.RegularExpressions;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    internal class HostSource {
        private readonly string mValue;

        public HostSource(string host) {
            if (host.IsNullOrWhiteSpace()) {
                throw new ArgumentException("The host parameter must be not empty and not null.");
            }
            ValidateHost(host);
            mValue = host.Trim().ToLower();
        }

        private static void ValidateHost(string host) {
            VerifyParts(SplitIntoHostSourceParts(host));
        }

        private static HostSourceParts SplitIntoHostSourceParts(string hostsource) {
            var parts = new HostSourceParts();
            string part = "";
            if(TryGetSchemePart(hostsource, out part)) {
                parts.Scheme = part;
                hostsource = hostsource.Substring(part.Length);
            }
            parts.Host = GetHostPart(hostsource);
            hostsource = hostsource.Substring(parts.Host.Length);
            if(TryGetPortPart(hostsource, out part)) {
                parts.Port = part;
                hostsource = hostsource.Substring(part.Length);
            }
            if(TryGetPathPart(hostsource, out part)) {
                parts.Path = part;
            }
            return parts;
        }

        private static bool TryGetSchemePart(string hostsource, out string scheme) {
            scheme = null;
            if(hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            int index = hostsource.IndexOf("://");
            if(index > 0) {
                scheme = hostsource.Substring(0, index + 3);
            }
            return index > 0;
        }

        private static string GetHostPart(string hostsource) {
            if(hostsource.IsNullOrWhiteSpace()) {
                return null;
            }
            string hostPart = hostsource;
            int index = hostsource.IndexOf(":");
            if(index > 0) {
                return hostsource.Substring(0, index);
            }
            index = hostsource.IndexOf("/");
            if(index > 0) {
                return hostsource.Substring(0, index);
            }
            return hostPart;
        }

        private static bool TryGetPortPart(string hostsource, out string port) {
            port = null;
            if(hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            if(hostsource[0] != ':') {
                return false;
            }
            int index = hostsource.IndexOf("/");
            port = index > 0 ? hostsource.Substring(0, index) : hostsource;
            return true;
        }

        private static bool TryGetPathPart(string hostsource, out string path) {
            path = null;
            if(hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            int index = hostsource.IndexOf("?");
            if(index > 0) {
                path = hostsource.Substring(0, index);
                return true;
            }
            index = hostsource.IndexOf("#");
            if(index > 0) {
                path = hostsource.Substring(0, index);
                return true;
            }
            path = hostsource;
            return true;
        }

        private static void VerifyParts(HostSourceParts parts) {
            VerifyHost(parts.Host);
            VerifyScheme(parts.Scheme);
            VerifyPort(parts.Port);
            VerifyPath(parts.Path);
        }

        private static void VerifyScheme(string scheme) {
            if(scheme.IsNullOrWhiteSpace()) {
                return;
            }
            const string schemeRegex = @"^[a-z][a-z0-9+\-.]*://$";
            if(RegexVerify(scheme, schemeRegex)) {
                return;
            }
            const string msg = "The extracted scheme '{0}' does not satisfy the required format.{1}" +
                               "Valid schemes:{1}ftp:// or a-12.adcd://{1}" +
                               "First character must be a letter and has to end with ://{1}" +
                               "For more informatin see: {2} (scheme-part).";
            throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyHost(string host) {
            if(host.IsNullOrWhiteSpace()) {
                throw new FormatException("At least the host-part is required.{1}For more information see: {2} (host-part)".FormatWith("http://www.w3.org/TR/CSP2/#host-source"));
            }
            const string hostRegex = @"^(\*(?!.)|(\*.)?[a-z0-9\-]+(?!\*)(\.[a-z0-9\-]+)*)$";
            if(RegexVerify(host, hostRegex)) {
                return;
            }
            const string msg = "The extracted host '{0}' does not satisfy the required format.{1}" +
                               "Valid hosts:{1}* or *.example or example.-com{1}" +
                               "For more information see: {2} (host-part).";
            throw new FormatException(msg.FormatWith(host, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyPort(string port) {
            if(port.IsNullOrWhiteSpace()) {
                return;
            }
            const string portRegex = @"^:([0-9]+|\*)$";
            if(RegexVerify(port, portRegex)) {
                return;
            }
            const string msg = "The extracted port '{0}' does not satisfy the required format.{1}" +
                               "Valid ports:{1}:* or :1234{1}" +
                               "First charater must be : (colon) followed by only a star or only digits.{1}" +
                               "For more information see: {2} (port-part).";
            throw new FormatException(msg.FormatWith(port, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyPath(string path) {
            if(path.IsNullOrWhiteSpace()) {
                return;
            }
            const string pathRegex = @" ^
                                        # RFC-3986 URI component:  path
                                    (?:                                                             # (
                                        (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*  #   path-abempty
                                    | /                                                             # / path-absolute
                                        (?:    (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})+
                                            (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
                                        )?
                                    |        (?:[A-Za-z0-9\-._~!$&'()*+,;=@] |%[0-9A-Fa-f]{2})+     # / path-noscheme
                                        (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
                                    |        (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})+     # / path-rootless
                                        (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
                                    |                                                               # / path-empty
                                    )                                                               # )
                                    $ ";
            if(RegexVerify(path, pathRegex, RegexOptions.IgnorePatternWhitespace)) {
                return;
            }
            const string msg = "The extracted path '{0}' does not satisfy the required format.{1}" +
                               "Valid paths:{1}/ or /path or /path/file.js" +
                               "For more information see: {2} (path-part).";
            throw new FormatException(msg.FormatWith(path, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }

        private static bool RegexVerify(string input, string pattern) {
            return RegexVerify(input, pattern, RegexOptions.None);
        }

        private static bool RegexVerify(string input, string pattern, RegexOptions options) {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | options);
        }


        public bool Equals(HostSource obj) {
            if (obj == null) {
                return false;
            }

            return mValue.Equals(obj.mValue, StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool Equals(object obj) {
            return Equals(obj as HostSource);
        }

        public override int GetHashCode() {
            return mValue.GetHashCode();
        }

        public override string ToString() {
            return mValue;
        }
    }
}