/*
 * Regex expression are from http://jmrware.com/articles/2009/uri_regexp/URI_regex.html
 */

using System;
using System.Collections.Generic;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Represents a source-list according to the CSP specification (http://www.w3.org/TR/CSP2/#source-list).
    /// </summary>
    public partial class CspSourceList : IDirectiveValueBuilder {
        private bool mIsNone;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CspSourceList" /> class.
        /// </summary>
        public CspSourceList() {
            mSchemes = new List<string>();
            mKeywords = new List<SourceListKeyword>();
            mHosts = new HostSourceCollection();
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
        ///     Sets the source-list to 'none'.<br />After this nothing can be added and will cause an
        ///     <see cref="InvalidOperationException" />.
        /// </summary>
        public void SetToNone() {
            mIsNone = true;
            mSchemes.Clear();
            mKeywords.Clear();
            mHosts.Clear();
        }

        private void ThrowIfNoneIsSet() {
            if (mIsNone) {
                throw new InvalidOperationException("This list ist set to 'none'. No additional values are allowed. Don't set this liste to 'none' when you need to add values.");
            }
        }

        private static string TrimAndEscape(string input) {
            return input.Trim().PercentEncode();
        }
    }
}