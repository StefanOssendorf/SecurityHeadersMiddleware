namespace SecurityHeadersMiddleware {
    /// <summary>
    /// Specifies the allowed source list keywords according to the CSP.<br/>
    /// See http://www.w3.org/TR/CSP2/#keyword-source
    /// </summary>
    public enum SourceListKeyword {
        /// <summary>
        /// Keyword: 'self'
        /// </summary>
        Self,
        /// <summary>
        /// Keyword: 'unsafe-inline'
        /// </summary>
        UnsafeInline,
        /// <summary>
        /// Keyword: 'unsafe-eval'
        /// </summary>
        UnsafeEval,
        /// <summary>
        /// Keyword: 'unsafe-redirect'
        /// </summary>
        UnsafeRedirect
    }
}