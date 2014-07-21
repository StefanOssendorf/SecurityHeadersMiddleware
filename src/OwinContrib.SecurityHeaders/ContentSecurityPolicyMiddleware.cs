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

            string schemeOfHostListRegex = @"([a-z][a-z0-9+\-.]*://)?";
            string hostOfHostListRegex = @"\*(?!.)|(\*.)?[a-z0-9\-]+(?!\*)(\.[a-z0-9\-]+)*";
            string portOfHostListRegex = @":((?!.)?([0-9]+|\*)(?!.))";


            // Parts validieren
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


            throw new NotImplementedException();
        }

        public void SetToNone() {
            mIsNone = true;
            mSchemes.Clear();
            mKeywords.Clear();
        }
        
        private HostSourceParts SplitIntoHostSourceParts(string hostsource) {
            var parts = new HostSourceParts();

            // Prüfen ob Scheme vorhanden
            string outTmp = "";
            if (TryGetSchemePart(hostsource, out outTmp)) {
                parts.Scheme = outTmp;
                hostsource = hostsource.Substring(outTmp.Length + 1);
            }

            // Prüfen ob Host vorhanden
            parts.Host = GetHostPart(hostsource);
            hostsource = hostsource.Substring(parts.Host.Length + 1);
            
            // Prüfen ob ein Path vorhanden ist

            return parts;
        }
        private string GetHostPart(string hostsource) {
            var hostPart = hostsource;

            var index = hostsource.IndexOf(":");
            if (index > 0) {
                hostPart = hostsource.Substring(0, index);
            }

            index = hostsource.IndexOf("/");
            if (index > 0) {
                hostPart = hostsource.Substring(0, index);
            }

            return hostPart;
        }
        private bool TryGetSchemePart(string hostsource, out string scheme) {
            scheme = null;
            const string schemeRegex = @"[a-z][a-z0-9+\-.]*://";
            var match = Regex.Match(hostsource, schemeRegex, RegexOptions.IgnoreCase);
            if (!match.Success) {
                return false;
            }
            scheme = match.Groups[0].Value;
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