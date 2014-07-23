namespace SecurityHeadersMiddleware {
    public class ContentSecurityPolicyConfiguration {
        /// <summary>
        /// Gets the base-uri directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-base-uri
        /// </summary>
        public CspSourceList BaseUri { get; private set; }
        /// <summary>
        /// Gets the child-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-child-src
        /// </summary>
        public CspSourceList ChildSrc { get; private set; }
        /// <summary>
        /// Gets the connect-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-connect-src
        /// </summary>
        public CspSourceList ConnectSrc { get; private set; }
        /// <summary>
        /// Gets the default-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-default-src
        /// </summary>
        public CspSourceList DefaultSrc { get; private set; }
        /// <summary>
        /// Gets the font-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-font-src
        /// </summary>
        public CspSourceList FontSrc { get; private set; }
        /// <summary>
        /// Gets the form-action directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-form-action
        /// </summary>
        public CspSourceList FormAction { get; private set; }
        /// <summary>
        /// Gets the frame-ancestors directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-frame-ancestors <br/>
        /// Info: According to the spec this directive replaces the X-Frame-Options header.
        /// </summary>
        public CspSourceList FrameAncestors { get; private set; }
        /// <summary>
        /// Gets the frame-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-frame-src
        /// </summary>
        public CspSourceList FrameSrc { get; private set; }
        /// <summary>
        /// Gets the img-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-img-src
        /// </summary>
        public CspSourceList ImgSrc { get; private set; }
        /// <summary>
        /// Gets the media-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-media-src
        /// </summary>
        public CspSourceList MediaSrc { get; private set; }
        /// <summary>
        /// Gets the object-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-object-src
        /// </summary>
        public CspSourceList ObjectSrc { get; private set; }
        /// <summary>
        /// Gets the plugin-types directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-plugin-types
        /// </summary>
        public CspSourceList PluginTypes { get; private set; }
        /// <summary>
        /// Gets the referrer directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP11/#directive-referrer
        /// </summary>
        public CspSourceList Referrer { get; private set; }
        /// <summary>
        /// Gets the reflected-xss directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-reflected-xss <br/>
        /// Info: "(...) subsume the functionality provided by the proprietary X-XSS-Protection HTTP header (...)"
        /// </summary>
        public CspSourceList ReflectedXss { get; private set; }
        /// <summary>
        /// Gets the report-uri directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-report-uri
        /// </summary>
        public CspSourceList ReportUri { get; private set; }
        /// <summary>
        /// Gets the sandbox directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-sandbox
        /// </summary>
        public CspSourceList Sandbox { get; private set; }
        /// <summary>
        /// Gets the script-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-script-src <br/>
        /// Info: Hash and Nonce not implemented yet.
        /// </summary>
        public CspSourceList ScriptSrc { get; private set; }
        /// <summary>
        /// Gets the style-src directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-style-src <br/>
        /// Info: Hash and Nonce not implemented yet.
        /// </summary>
        public CspSourceList StyleSrc { get; set; }
    }
}