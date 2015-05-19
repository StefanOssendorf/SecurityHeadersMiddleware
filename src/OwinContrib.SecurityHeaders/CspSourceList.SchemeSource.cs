using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    partial class CspSourceList {
        private readonly List<string> mSchemes;

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
            var match = Regex.Match(scheme, schemeRegex, RegexOptions.IgnoreCase);
            if (!match.Success) {
                const string msg = "Scheme value '{0}' is invalid.{1}" +
                                   "Valid schemes:{1}http: or http or ftp: or ftp{1}" +
                                   "First charachter must be a letter.{1}" +
                                   "For more Information see: {2}";
                throw new FormatException(msg.FormatWith(scheme, Environment.NewLine, "http://www.w3.org/TR/CSP2/#scheme-source"));
            }
            var schemeToAdd = "{0}:".FormatWith(match.Groups[1].Value.ToLower());
            if (mSchemes.Contains(schemeToAdd)) {
                return;
            }
            mSchemes.Add(schemeToAdd);
        }

        private string BuildSchemeValues() {
            return string.Join(" ", mSchemes);
        }
    }
}