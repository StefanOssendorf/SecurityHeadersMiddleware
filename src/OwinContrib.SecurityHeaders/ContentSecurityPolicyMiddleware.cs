using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    internal static class ContentSecurityPolicyMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentSecurityPolicyHeader() {
            return next =>
                env => {
                    return next(env);
                };
        }
    }

    public class ContentSecurityPolicyConfiguration {
        public string DefaultSrc { get; set; }
        public string ScriptSrc { get; set; }
    }


    public enum CspKeyword {
        Self,
        UnsafeInline,
        UnsafeEval,
        UnsafeRedirect
    }


    /*TODO:
     * Implementing source expression types:
     * - Host-Source
     * 
     * Finished:
     * - Scheme-Source
     * - 'None' value
     * - Keyword-Source
     * 
     * Excluded:
     * - Nonce-Source
     * - Hash-Souce
     * 
     */

    public class CspSourceList {
        private bool mIsNone;
        private readonly List<string> mSchemes;
        private readonly List<CspKeyword> mKeywords;
        private const string SchemeRegexExact = @"(^[a-z][a-z0-9+\-.]*)(:?)$";

        public CspSourceList() {
            mSchemes = new List<string>();
            mKeywords = new List<CspKeyword>();
            mIsNone = false;
        }

        public void AddKeyword(CspKeyword keyword) {
            ThrowIfNoneIsSet();
            if (mKeywords.Contains(keyword)) {
                return;
            }
            mKeywords.Add(keyword);
        }

        public void AddScheme(string scheme) {
            ThrowIfNoneIsSet();
            scheme.MustNotNull("scheme");
            scheme = scheme.ToLower();

            var match = Regex.Match(scheme, SchemeRegexExact, RegexOptions.IgnoreCase);
            if (!match.Success) {
                var message = "Scheme value '{0}' is invalid.{1}Valid schemes:{1}http: or http or ftp: or ftp{1}First charachter must be a letter.{1}For more Information see: {2}".FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#scheme-source");
                throw new FormatException(message);
            }

            var schemeToAdd = "{0}:".FormatWith(match.Groups[1].Value.ToLower());
            if (mSchemes.Contains(schemeToAdd)) {
                return;
            }
            mSchemes.Add(schemeToAdd);
        }

        public void AddHost(Uri host) {
            AddHost(host.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped));
        }

        public void AddHost(string host) {
            ThrowIfNoneIsSet();
            host.MustNotNull("host");
            host.MustNotBeWhiteSpace("host");

            var parts = SplitIntoHostSourceParts(host);
            VerifyParts(parts);

            
            
            string portOfHostListRegex = @":((?!.)?([0-9]+|\*)(?!.))";           


            var hostPart = parts.Host;
            if (hostPart.Contains("*") && hostPart.IndexOf("*") != 0) {
                throw new FormatException();
            }

            if (!Regex.IsMatch(hostPart, @"[a-z0-9\-]$")) {
                throw new FormatException();
            }

            const string hostRegex = @"(\*(?!.)|(\*.)?[a-z0-9\-]+(\.[a-z0-9\-]+)*)";
            if (!Regex.IsMatch(hostPart, hostRegex)) {
                throw new FormatException();
            }
        }
        private void VerifyParts(HostSourceParts parts) {
            VerifyHost(parts.Host);
            VerifyScheme(parts.Scheme);
            VerifyPort(parts.Port);
            VerifyPath(parts.Path);
        }
        private void VerifyScheme(string scheme) {
            if (scheme.IsNullOrWhiteSpace()) {
                return;
            }
            const string schemeRegex = @"^[a-z][a-z0-9+\-.]*://$";
            if (RegexVerify(scheme, schemeRegex)) {
                return;
            }
            const string msg =  "The extracted scheme '{0}' does not satisfy the required format.{1}" +
                                "Valid schemes:{1}ftp:// or a-12.adcd://{1}" + 
                                "First character must be a letter and has to end with ://{1}" +
                                "For more informatin see: {2} (scheme-part definition from host-source)";

            throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private void VerifyHost(string host) {
            if (host.IsNullOrWhiteSpace()) {
                return;
            }
            const string hostRegex = @"^(\*(?!.)|(\*.)?[a-z0-9\-]+(?!\*)(\.[a-z0-9\-]+)*)$";
            if (RegexVerify(host, hostRegex)) {
                return;
            }
            const string msg = "The extracted host '{0}' does not satisfy the required format.{1}" +
                               "Valid hosts:{1}* or *.example or example.-com{1}" +
                               "For more information see:{2} (host-part)";
            throw new FormatException(msg.FormatWith(host, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-part"));
        }
        private void VerifyPort(string port) {
            throw  new FormatException();
        }
        private void VerifyPath(string path) {
            throw new FormatException();
        }

        private bool RegexVerify(string input, string pattern) {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        }

        public void SetToNone() {
            mIsNone = true;
            mSchemes.Clear();
            mKeywords.Clear();
        }

        private HostSourceParts SplitIntoHostSourceParts(string hostsource) {
            var parts = new HostSourceParts();

            string part = "";
            if (TryGetSchemePart(hostsource, out part)) {
                parts.Scheme = part;
                hostsource = hostsource.Substring(part.Length);
            }

            parts.Host = GetHostPart(hostsource);
            hostsource = hostsource.Substring(parts.Host.Length);

            if (TryGetPortPart(hostsource, out part)) {
                parts.Port = part;
                hostsource = hostsource.Substring(part.Length);
            }

            if (TryGetPathPart(hostsource, out part)) {
                parts.Path = part;
            }

            return parts;
        }

        private bool TryGetSchemePart(string hostsource, out string scheme) {
            scheme = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }

            var index = hostsource.IndexOf("://");
            if (index > 0) {
                scheme = hostsource.Substring(0, index + 3);
            }
            return index > 0;
        }
        private string GetHostPart(string hostsource) {
            if (hostsource.IsNullOrWhiteSpace()) {
                return null;
            }
            var hostPart = hostsource;

            var index = hostsource.IndexOf(":");
            if (index > 0) {
                return hostsource.Substring(0, index);
            }

            index = hostsource.IndexOf("/");
            if (index > 0) {
                return hostsource.Substring(0, index);
            }

            return hostPart;
        }
        private bool TryGetPortPart(string hostsource, out string port) {
            port = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            if (hostsource[0] != ':') {
                return false;
            }
            var index = hostsource.IndexOf("/");
            port = index > 0 ? hostsource.Substring(0, index) : hostsource;
            return true;
        }
        private bool TryGetPathPart(string hostsource, out string path) {
            path = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            var index = hostsource.IndexOf("?");
            
            if (index > 0) {
                path = hostsource.Substring(0, index);
                return true;
            }
            index = hostsource.IndexOf("#");
            if (index > 0) {
                path = hostsource.Substring(0, index);
                return true;
            }

            path = hostsource;
            return true;
        }

        internal void AddNonce(string nonce) {
        }
        internal void AddHash(string hash) {
        }


        private void ThrowIfNoneIsSet() {
            if (mIsNone) {
                throw new InvalidOperationException("This list ist set to 'none'. No additional values are allowed. Don't set this liste to 'none' to add new values.");
            }
        }
        internal string ToHeaderValue() {
            if (mIsNone) {
                return "'none'";
            }

            var sb = new StringBuilder();

            sb.AppendFormat(" {0} ", BuildSchemeValues());
            sb.AppendFormat(" {0} ", BuildKeywordValues());

            return sb.ToString().Trim();
        }
        private string BuildKeywordValues() {
            var sb = new StringBuilder();
            foreach (var keyword in mKeywords) {
                sb.AppendFormat("'{0}' ", keyword.ToString().ToLower());
            }
            return sb.ToString();
        }
        private string BuildSchemeValues() {
            var sb = new StringBuilder();
            foreach (var scheme in mSchemes) {
                sb.AppendFormat("{0} ", scheme);
            }
            return sb.ToString();
        }
    }

    internal class HostSourceParts {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }

        public HostSourceParts() {
            Scheme = "";
            Host = null;
            Port = "";
            Path = "";
        }
    }
}