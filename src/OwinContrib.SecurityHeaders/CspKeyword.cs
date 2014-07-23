namespace SecurityHeadersMiddleware {
    /// <summary>
    /// Specifies the allowed keywords through CSP.<br/>
    /// See http://www.w3.org/TR/CSP2/#keyword-source
    /// </summary>
    public enum CspKeyword {
        /// <summary>
        /// Keyword: 'self'
        /// </summary>
        Self,
        /// <summary>
        /// Keyword: 'unsafe-inline'
        /// </summary>
        UnsafeInline,
        UnsafeEval,
        UnsafeRedirect
    }
}