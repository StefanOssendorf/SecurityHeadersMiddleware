/*
 * Regex expression are from http://jmrware.com/articles/2009/uri_regexp/URI_regex.html
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Represents a source-list according to the CSP specification (http://www.w3.org/TR/CSP2/#source-list).
    /// </summary>
    public class CspSourceList : IDirectiveValueBuilder {
        private readonly List<string> mHosts;
        private readonly List<SourceListKeyword> mKeywords;
        private readonly List<string> mSchemes;
        private bool mIsNone;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CspSourceList" /> class.
        /// </summary>
        public CspSourceList() {
            mSchemes = new List<string>();
            mKeywords = new List<SourceListKeyword>();
            mHosts = new List<string>();
            mIsNone = false;
        }
        /// <summary>
        ///     Creates the directive header value.
        /// </summary>
        /// <returns>The directive header value without directive-name.</returns>
        public string ToDirectiveValue() {
            if (mIsNone) {
                return "'none'";
            }
            var sb = new StringBuilder();
            sb.AppendFormat(" {0} ", TrimAndEscape(BuildSchemeValues()));
            sb.AppendFormat(" {0} ", TrimAndEscape(BuildHostValues()));
            sb.AppendFormat(" {0} ", TrimAndEscape(BuildKeywordValues()));
            return sb.ToString().Trim();
        }

        /// <summary>
        ///     Adds a keyword to the source-list.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        public void AddKeyword(SourceListKeyword keyword) {
            ThrowIfNoneIsSet();
            if (mKeywords.Contains(keyword)) {
                return;
            }
            mKeywords.Add(keyword);
        }

        /// <summary>
        ///     Adds a scheme to the source-list.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <exception cref="System.FormatException">
        ///     <paramref name="scheme" /> has to satisfy this regex: ^[a-z][a-z0-9+\-.]*:?$ <br />
        ///     For more information see http://www.w3.org/TR/CSP2/#scheme-source
        /// </exception>
        public void AddScheme(string scheme) {
            ThrowIfNoneIsSet();
            scheme.MustNotNull("scheme");
            scheme = scheme.ToLower();
            const string schemeRegex = @"(^[a-z][a-z0-9+\-.]*)(:?)$";
            Match match = Regex.Match(scheme, schemeRegex, RegexOptions.IgnoreCase);
            if (!match.Success) {
                const string msg = "Scheme value '{0}' is invalid.{1}" +
                                   "Valid schemes:{1}http: or http or ftp: or ftp{1}" +
                                   "First charachter must be a letter.{1}" +
                                   "For more Information see: {2}";
                throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#scheme-source"));
            }
            string schemeToAdd = "{0}:".FormatWith(match.Groups[1].Value.ToLower());
            if (mSchemes.Contains(schemeToAdd)) {
                return;
            }
            mSchemes.Add(schemeToAdd);
        }

        /// <summary>
        ///     Adds a host to the source-list.
        /// </summary>
        /// <param name="host">The host.</param>
        public void AddHost(Uri host) {
            AddHost(host.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped));
        }
        /// <summary>
        ///     Adds a host to the source-list.
        /// </summary>
        /// <param name="host">The host.</param>
        public void AddHost(string host) {
            ThrowIfNoneIsSet();
            host.MustNotNull("host");
            host.MustNotBeWhiteSpaceOrEmpty("host");
            host = host.ToLower();
            HostSourceParts parts = SplitIntoHostSourceParts(host);
            VerifyParts(parts);
            if (mHosts.Contains(host)) {
                return;
            }
            mHosts.Add(host);
        }
        private static void VerifyParts(HostSourceParts parts) {
            VerifyHost(parts.Host);
            VerifyScheme(parts.Scheme);
            VerifyPort(parts.Port);
            VerifyPath(parts.Path);
        }
        private static void VerifyScheme(string scheme) {
            if (scheme.IsNullOrWhiteSpace()) {
                return;
            }
            const string schemeRegex = @"^[a-z][a-z0-9+\-.]*://$";
            if (RegexVerify(scheme, schemeRegex)) {
                return;
            }
            const string msg = "The extracted scheme '{0}' does not satisfy the required format.{1}" +
                               "Valid schemes:{1}ftp:// or a-12.adcd://{1}" +
                               "First character must be a letter and has to end with ://{1}" +
                               "For more informatin see: {2} (scheme-part).";
            throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyHost(string host) {
            if (host.IsNullOrWhiteSpace()) {
                throw new FormatException("At least the host-part is required.{1}For more information see: {2} (host-part)".FormatWith("http://www.w3.org/TR/CSP2/#host-source"));
            }
            const string hostRegex = @"^(\*(?!.)|(\*.)?[a-z0-9\-]+(?!\*)(\.[a-z0-9\-]+)*)$";
            if (RegexVerify(host, hostRegex)) {
                return;
            }
            const string msg = "The extracted host '{0}' does not satisfy the required format.{1}" +
                               "Valid hosts:{1}* or *.example or example.-com{1}" +
                               "For more information see: {2} (host-part).";
            throw new FormatException(msg.FormatWith(host, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyPort(string port) {
            if (port.IsNullOrWhiteSpace()) {
                return;
            }
            const string portRegex = @"^:([0-9]+|\*)$";
            if (RegexVerify(port, portRegex)) {
                return;
            }
            const string msg = "The extracted port '{0}' does not satisfy the required format.{1}" +
                               "Valid ports:{1}:* or :1234{1}" +
                               "First charater must be : (colon) followed by only a star or only digits.{1}" +
                               "For more information see: {2} (port-part).";
            throw new FormatException(msg.FormatWith(port, Environment.NewLine, "http://www.w3.org/TR/CSP2/#host-source"));
        }
        private static void VerifyPath(string path) {
            if (path.IsNullOrWhiteSpace()) {
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
            if (RegexVerify(path, pathRegex, RegexOptions.IgnorePatternWhitespace)) {
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

        /// <summary>
        ///     Sets the source-list to 'none'.<br />After this nothing can be added and will cause an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        public void SetToNone() {
            mIsNone = true;
            mSchemes.Clear();
            mKeywords.Clear();
            mHosts.Clear();
        }

        private static HostSourceParts SplitIntoHostSourceParts(string hostsource) {
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

        private static bool TryGetSchemePart(string hostsource, out string scheme) {
            scheme = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            int index = hostsource.IndexOf("://");
            if (index > 0) {
                scheme = hostsource.Substring(0, index + 3);
            }
            return index > 0;
        }
        private static string GetHostPart(string hostsource) {
            if (hostsource.IsNullOrWhiteSpace()) {
                return null;
            }
            string hostPart = hostsource;
            int index = hostsource.IndexOf(":");
            if (index > 0) {
                return hostsource.Substring(0, index);
            }
            index = hostsource.IndexOf("/");
            if (index > 0) {
                return hostsource.Substring(0, index);
            }
            return hostPart;
        }
        private static bool TryGetPortPart(string hostsource, out string port) {
            port = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            if (hostsource[0] != ':') {
                return false;
            }
            int index = hostsource.IndexOf("/");
            port = index > 0 ? hostsource.Substring(0, index) : hostsource;
            return true;
        }
        private static bool TryGetPathPart(string hostsource, out string path) {
            path = null;
            if (hostsource.IsNullOrWhiteSpace()) {
                return false;
            }
            int index = hostsource.IndexOf("?");
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
            //TODO: Maybe later...
        }
        internal void AddHash(string hash) {
            //TODO: Maybe later...
        }

        private void ThrowIfNoneIsSet() {
            if (mIsNone) {
                throw new InvalidOperationException("This list ist set to 'none'. No additional values are allowed. Don't set this liste to 'none' to add new values.");
            }
        }

        private string BuildSchemeValues() {
            var sb = new StringBuilder();
            foreach (string scheme in mSchemes) {
                sb.AppendFormat("{0} ", scheme);
            }
            return sb.ToString();
        }
        private string BuildHostValues() {
            var sb = new StringBuilder();
            foreach (string host in mHosts) {
                sb.AppendFormat("{0} ", host.ToLower());
            }
            return sb.ToString();
        }
        private string BuildKeywordValues() {
            var sb = new StringBuilder();
            foreach (SourceListKeyword keyword in mKeywords) {
                string value = keyword.ToString().ToLower();
                if (value.StartsWith("unsafe")) {
                    value = value.Insert(6, "-");
                }
                sb.AppendFormat("'{0}' ", value);
            }
            return sb.ToString();
        }

        private static string TrimAndEscape(string input) {
            return input.Trim().Replace(";", "%3B").Replace(",", "%2C");
        }
    }
}