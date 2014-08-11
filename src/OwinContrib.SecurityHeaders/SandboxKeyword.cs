namespace SecurityHeadersMiddleware {
    /// <summary>
    /// Specifies the allowed keywords according to the CSP.<br/>
    /// See: http://www.w3.org/TR/CSP2/#directive-sandbox , http://developers.whatwg.org/the-iframe-element.html#attr-iframe-sandbox and http://lists.w3.org/Archives/Public/public-webappsec/2014Aug/0019.html
    /// </summary>
    public enum SandboxKeyword {
        /// <summary>
        /// Keyword: allow-forms
        /// </summary>
        AllowForms,
        /// <summary>
        /// Keyword: allow-pointer-lock
        /// </summary>
        AllowPointerLock,
        /// <summary>
        /// Keyword: allow-popups
        /// </summary>
        AllowPopups,
        /// <summary>
        /// Keyword: allow-same-origin
        /// </summary>
        AllowSameOrigin,
        /// <summary>
        /// Keyword: allow-scripts
        /// </summary>
        AllowScripts,
        /// <summary>
        /// Keyword: allow-top-navigation
        /// </summary>
        AllowTopNavigation
    }
}