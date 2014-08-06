using System;

namespace SecurityHeadersMiddleware {
    /// <summary>
    /// Specifies the allowed keywords according to the CSP.<br/>
    /// See: http://www.w3.org/TR/CSP2/#directive-referrer
    /// </summary>
    public enum ReferrerKeyword {
        /// <summary>
        /// Default value for indicating the directive value is not set and omitted.
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// Keyword: none
        /// </summary>
        None,
        /// <summary>
        /// Keyword: none-when-downgrade
        /// </summary>
        NoneWhenDowngrade,
        /// <summary>
        /// Keyword: origin
        /// </summary>
        Origin,
        /// <summary>
        /// Keyword: origin-when-cross-origin
        /// </summary>
        OriginWhenCossOrigin,
        /// <summary>
        /// Keyword: unsafe-url
        /// </summary>
        UnsafeUrl
    }
}