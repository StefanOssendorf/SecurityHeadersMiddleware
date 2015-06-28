using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecurityHeadersMiddleware.Infrastructure;


namespace SecurityHeadersMiddleware {
    internal class HostSource {
        private static readonly Dictionary<string, int> DefaultPorts = new Dictionary<string, int> {
            {"http://", 80}, {"https://", 443}, {"ftp://", 21}
        };

        private string mScheme = "";
        private string mHost = "";
        private string mPort = "";
        private string mPath = "";

        private string Value {
            get { return mScheme + mHost + mPort + mPath; }
        }

        public HostSource(string host) {
            if (host.IsNullOrWhiteSpace()) {
                throw new ArgumentException("The host parameter must be not empty, not null and not only whitespaces.");
            }
            ParseHost(host);
        }

        private void ParseHost(string host) {
            SplitIntoParts(host);
            VerifyParts();
        }

        private void SplitIntoParts(string hostsource) {
            ExtractScheme(ref hostsource);
            ExtractHost(ref hostsource);
            ExtractPort(ref hostsource);
            ExtractPath(ref hostsource);
        }

        private void ExtractScheme(ref string hostsource) {
            var index = hostsource.IndexOf("://");
            if (index == -1) {
                return;
            }
            mScheme = hostsource.Substring(0, index + 3);
            hostsource = hostsource.Substring(mScheme.Length);
        }

        private void ExtractHost(ref string hostsource) {
            var index = hostsource.IndexOf(":");
            if (index == -1) {
                index = hostsource.IndexOf("/");
                if (index == -1) {
                    index = hostsource.Length;
                }
            }

            mHost = hostsource.Substring(0, index);
            hostsource = hostsource.Substring(mHost.Length);
        }

        private void ExtractPort(ref string hostsource) {
            if (hostsource.IsEmpty()) {
                return;
            }
            if (hostsource[0] != ':') {
                return;
            }
            var index = hostsource.IndexOf("/");
            if (index == -1) {
                index = hostsource.Length;
            }
            mPort = hostsource.Substring(0, index);
            hostsource = hostsource.Substring(mPort.Length);
            int defaultPort;
            if (DefaultPorts.TryGetValue(mScheme, out defaultPort)) {
                // Omit default port for easier comparison
                if (mPort == ":" + defaultPort) {
                    mPort = "";
                }
            }
        }

        private void ExtractPath(ref string hostsource) {
            if (hostsource.IsEmpty()) {
                return;
            }
            var index = hostsource.IndexOf("?");
            if (index == -1) {
                index = hostsource.IndexOf("#");
                if (index == -1) {
                    index = hostsource.Length;
                }
            }
            mPath = hostsource.Substring(0, index);
            hostsource = "";
        }

        private void VerifyParts() {
            VerifyHost();
            VerifyScheme();
            VerifyPort();
            VerifyPath();
        }

        private void VerifyScheme() {
            if (mScheme.IsEmpty()) {
                return;
            }
            const string schemeRegex = @"^[a-z][a-z0-9+\-.]*://$";
            if (RegexVerify(mScheme, schemeRegex)) {
                return;
            }
            const string msg = "The extracted scheme '{0}' does not satisfy the required format.{1}" +
                               "Valid schemes:{1}ftp:// or a-12.adcd://{1}" +
                               "First character must be a letter and has to end with ://{1}" +
                               "For more informatin see: {2} (scheme-part).";
            throw new FormatException(msg.FormatWith(mScheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }

        private void VerifyHost() {
            const string hostRegex = @"^(\*(?!.)|(\*.)?[a-z0-9\-]+(?!\*)(\.[a-z0-9\-]+)*)$";
            if (RegexVerify(mHost, hostRegex)) {
                return;
            }
            const string msg = "The extracted host '{0}' does not satisfy the required format.{1}" +
                               "Valid hosts:{1}* or *.example or example.-com{1}" +
                               "For more information see: {2} (host-part).";
            throw new FormatException(msg.FormatWith(mHost, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }

        private void VerifyPort() {
            if (mPort.IsEmpty()) {
                return;
            }
            const string portRegex = @"^:([0-9]+|\*)$";
            if (RegexVerify(mPort, portRegex)) {
                return;
            }
            const string msg = "The extracted port '{0}' does not satisfy the required format.{1}" +
                               "Valid ports:{1}:* or :1234{1}" +
                               "First charater must be : (colon) followed by only a star or only digits.{1}" +
                               "For more information see: {2} (port-part).";
            throw new FormatException(msg.FormatWith(mPort, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }

        private void VerifyPath() {
            if (mPath.IsEmpty()) {
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
            if (RegexVerify(mPath, pathRegex, RegexOptions.IgnorePatternWhitespace)) {
                return;
            }
            const string msg = "The extracted path '{0}' does not satisfy the required format.{1}" +
                               "Valid paths:{1}/ or /path or /path/file.js" +
                               "For more information see: {2} (path-part).";
            throw new FormatException(msg.FormatWith(mPath, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }

        private static bool RegexVerify(string input, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | options);
        }

        public bool Equals(HostSource obj) {
            if (obj == null) {
                return false;
            }

            return Value.Equals(obj.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj) {
            return Equals(obj as HostSource);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public override string ToString() {
            return Value;
        }
    }
}