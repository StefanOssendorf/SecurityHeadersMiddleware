using System;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Specifies the allowed keywords according to the CSP.<br />
    ///     See: http://www.w3.org/TR/CSP2/#directive-referrer
    /// </summary>
    public enum ReferrerKeyword {
        /// <summary>
        ///     Default value for indicating the directive value is not set and omitted.
        /// </summary>
        NotSet = 0,
        /// <summary>
        ///     Keyword: none
        /// </summary>
        [Obsolete("No warning, just an info! Will be obsolete with the next CSP version. It will be replaced by 'no-referrer'.")]
        None,
        /// <summary>
        ///     Keyword: none-when-downgrade
        /// </summary>
        NoneWhenDowngrade,
        /// <summary>
        ///     Keyword: origin
        /// </summary>
        Origin,
        /// <summary>
        ///     Keyword: origin-when-cross-origin
        /// </summary>
        OriginWhenCossOrigin,
        /// <summary>
        ///     Keyword: unsafe-url
        /// </summary>
        UnsafeUrl
    }
}