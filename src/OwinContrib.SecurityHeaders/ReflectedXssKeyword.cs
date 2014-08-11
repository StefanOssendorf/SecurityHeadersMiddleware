namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Specifies the allowed keywords according to the CSP.<br />
    ///     See: http://www.w3.org/TR/CSP2/#directive-reflected-xss
    /// </summary>
    public enum ReflectedXssKeyword {
        /// <summary>
        ///     Default value for indicating the directive value is not set and omitted.
        /// </summary>
        NotSet = 0,
        /// <summary>
        ///     Keyword: allow <br />
        ///     This keyword DISABLES the user agents active protection. It's equivalent to:  X-XSS-Protection: 0
        /// </summary>
        Allow,
        /// <summary>
        ///     Keyword: block <br />
        ///     It's equivalent to X-XSS-Protection: 1; mode=block
        /// </summary>
        Block,
        /// <summary>
        ///     Keyword: filter <br />
        ///     It's equivalent to X-XSS-Protection: 1
        /// </summary>
        Filter
    }
}