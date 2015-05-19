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
    ///     Represents an uri-reference-list according to the CSP specification
    ///     http://www.w3.org/TR/CSP2/#directive-report-uri).
    /// </summary>
    public class CspUriReferenceList : IDirectiveValueBuilder {
        #region Regex

        private static readonly Regex Rfc3986UriRegex = new Regex(@" ^
    # RFC-3986 URI component:  URI
    [A-Za-z][A-Za-z0-9+\-.]* :                                      # scheme "":""
    (?: //                                                          # hier-part
      (?: (?:[A-Za-z0-9\-._~!$&'()*+,;=:]|%[0-9A-Fa-f]{2})* @)?
      (?:
        \[
        (?:
          (?:
            (?:                                                    (?:[0-9A-Fa-f]{1,4}:){6}
            |                                                   :: (?:[0-9A-Fa-f]{1,4}:){5}
            | (?:                            [0-9A-Fa-f]{1,4})? :: (?:[0-9A-Fa-f]{1,4}:){4}
            | (?: (?:[0-9A-Fa-f]{1,4}:){0,1} [0-9A-Fa-f]{1,4})? :: (?:[0-9A-Fa-f]{1,4}:){3}
            | (?: (?:[0-9A-Fa-f]{1,4}:){0,2} [0-9A-Fa-f]{1,4})? :: (?:[0-9A-Fa-f]{1,4}:){2}
            | (?: (?:[0-9A-Fa-f]{1,4}:){0,3} [0-9A-Fa-f]{1,4})? ::    [0-9A-Fa-f]{1,4}:
            | (?: (?:[0-9A-Fa-f]{1,4}:){0,4} [0-9A-Fa-f]{1,4})? ::
            ) (?:
                [0-9A-Fa-f]{1,4} : [0-9A-Fa-f]{1,4}
              | (?: (?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?) \.){3}
                    (?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)
              )
          |   (?: (?:[0-9A-Fa-f]{1,4}:){0,5} [0-9A-Fa-f]{1,4})? ::    [0-9A-Fa-f]{1,4}
          |   (?: (?:[0-9A-Fa-f]{1,4}:){0,6} [0-9A-Fa-f]{1,4})? ::
          )
        | [Vv][0-9A-Fa-f]+\.[A-Za-z0-9\-._~!$&'()*+,;=:]+
        )
        \]
      | (?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}
           (?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)
      | (?:[A-Za-z0-9\-._~!$&'()*+,;=]|%[0-9A-Fa-f]{2})*
      )
      (?: : [0-9]* )?
      (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
    | /
      (?:    (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})+
        (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
      )?
    |        (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})+
        (?:/ (?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})* )*
    |
    )
    (?:\? (?:[A-Za-z0-9\-._~!$&'()*+,;=:@/?]|%[0-9A-Fa-f]{2})* )?   # [ ""?"" query ]
    (?:\# (?:[A-Za-z0-9\-._~!$&'()*+,;=:@/?]|%[0-9A-Fa-f]{2})* )?   # [ ""#"" fragment ]
    $ ", RegexOptions.IgnorePatternWhitespace);

        #endregion

        private readonly List<string> mUris;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CspUriReferenceList" /> class.
        /// </summary>
        public CspUriReferenceList() {
            mUris = new List<string>();
        }

        /// <summary>
        ///     Creates the directive header value.
        /// </summary>
        /// <returns>The directive header value without directive-name.</returns>
        public string ToDirectiveValue() {
            var sb = new StringBuilder();
            foreach (var uri in mUris) {
                sb.AppendFormat(" {0} ", uri);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Adds a uri to the uri-reference-list.
        /// </summary>
        /// <param name="uri">The uri.</param>
        public void AddReportUri(Uri uri) {
            AddReportUri(uri.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped));
        }

        /// <summary>
        ///     Adds a uri to the uri-reference-list.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <exception cref="System.FormatException">
        ///     <paramref name="uri" /> has to satisfy the URI definition.
        ///     For more information see: http://www.w3.org/TR/CSP2/#uri-reference respectively
        ///     http://tools.ietf.org/html/rfc3986#section-3.3
        /// </exception>
        public void AddReportUri(string uri) {
            uri.MustNotNull("uri");
            uri.MustNotBeWhiteSpaceOrEmpty("uri");
            if (!Rfc3986UriRegex.IsMatch(uri)) {
                //TODO: Exceptionmessage
                throw new FormatException();
            }
            uri = uri.ToLower();
            if (mUris.Contains(uri)) {
                return;
            }
            mUris.Add(uri);
        }
    }
}