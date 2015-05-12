using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    //TODO: Implement
    public class CspAncestorSourceList : IDirectiveValueBuilder {
        private bool mIsNone;
        private readonly List<string> mSchemes;
        private readonly List<string> mHosts;

        public CspAncestorSourceList() {
            mSchemes = new List<string>();
            mHosts = new List<string>();
            mIsNone = false;
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
            if(!match.Success) {
                const string msg = "Scheme value '{0}' is invalid.{1}" +
                                   "Valid schemes:{1}http: or http or ftp: or ftp{1}" +
                                   "First charachter must be a letter.{1}" +
                                   "For more Information see: {2}";
                throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#scheme-source"));
            }
            string schemeToAdd = "{0}:".FormatWith(match.Groups[1].Value.ToLower());
            if(mSchemes.Contains(schemeToAdd)) {
                return;
            }
            mSchemes.Add(schemeToAdd);
        }

        private void ThrowIfNoneIsSet() {
            if (mIsNone) {
                throw new InvalidOperationException("This list ist set to 'none'. No additional values are allowed. Don't set this liste to 'none' when you need to add values.");
            }
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
            if(mHosts.Contains(host)) {
                return;
            }
            mHosts.Add(host);
        }

        private void VerifyParts(HostSourceParts parts) {
            throw new NotImplementedException();
        }

        private HostSourceParts SplitIntoHostSourceParts(string host) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Sets the ancestor source-list to 'none'.<br />After this nothing can be added and will cause an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        public void SetToNone() {
            mSchemes.Clear();
            mHosts.Clear();
            mIsNone = true;
        }

        /// <summary>
        ///     Creates the directive header value.
        /// </summary>
        /// <returns>The directive header value without directive-name.</returns>
        public string ToDirectiveValue() {
            if (mIsNone) {
                return "'none'";
            }
            throw new NotImplementedException();
        }
    }
}