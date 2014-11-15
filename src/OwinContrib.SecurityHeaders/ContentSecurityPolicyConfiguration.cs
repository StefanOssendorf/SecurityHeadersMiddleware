using System.Collections.Generic;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Defines the content-security-policy header values.
    /// </summary>
    public class ContentSecurityPolicyConfiguration {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContentSecurityPolicyConfiguration" /> class.
        /// </summary>
        public ContentSecurityPolicyConfiguration() {
            BaseUri = new CspSourceList();
            ChildSrc = new CspSourceList();
            ConnectSrc = new CspSourceList();
            DefaultSrc = new CspSourceList();
            FontSrc = new CspSourceList();
            FormAction = new CspSourceList();
            FrameAncestors = new CspSourceList();
            FrameSrc = new CspSourceList();
            ImgSrc = new CspSourceList();
            MediaSrc = new CspSourceList();
            ObjectSrc = new CspSourceList();
            ScriptSrc = new CspSourceList();
            StyleSrc = new CspSourceList();
            PluginTypes = new CspMediaTypeList();
            Referrer = ReferrerKeyword.NotSet;
            ReflectedXss = ReflectedXssKeyword.NotSet;
            ReportUri = new CspUriReferenceList();
            Sandbox = new CspSandboxTokenList();
        }
        /// <summary>
        ///     Gets the base-uri directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-base-uri
        /// </summary>
        public CspSourceList BaseUri { get; private set; }
        /// <summary>
        ///     Gets the child-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-child-src
        /// </summary>
        public CspSourceList ChildSrc { get; private set; }
        /// <summary>
        ///     Gets the connect-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-connect-src
        /// </summary>
        public CspSourceList ConnectSrc { get; private set; }
        /// <summary>
        ///     Gets the default-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-default-src
        /// </summary>
        public CspSourceList DefaultSrc { get; private set; }
        /// <summary>
        ///     Gets the font-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-font-src
        /// </summary>
        public CspSourceList FontSrc { get; private set; }
        /// <summary>
        ///     Gets the form-action directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-form-action
        /// </summary>
        public CspSourceList FormAction { get; private set; }
        
        //TODO: FrameAncesotrs own source-list +.+
        /// <summary>
        ///     Gets the frame-ancestors directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-frame-ancestors <br />
        ///     Info: According to the spec this directive replaces the X-Frame-Options header.
        /// </summary>
        public CspSourceList FrameAncestors { get; private set; }
        /// <summary>
        ///     Gets the frame-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-frame-src
        /// </summary>
        public CspSourceList FrameSrc { get; private set; }
        /// <summary>
        ///     Gets the img-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-img-src
        /// </summary>
        public CspSourceList ImgSrc { get; private set; }
        /// <summary>
        ///     Gets the media-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-media-src
        /// </summary>
        public CspSourceList MediaSrc { get; private set; }
        /// <summary>
        ///     Gets the object-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-object-src
        /// </summary>
        public CspSourceList ObjectSrc { get; private set; }

        /// <summary>
        ///     Gets the plugin-types directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-plugin-types
        /// </summary>
        public CspMediaTypeList PluginTypes { get; private set; }

        /// <summary>
        ///     Gets the referrer directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP11/#directive-referrer
        /// </summary>
        public ReferrerKeyword Referrer { get; set; }

        /// <summary>
        ///     Gets the reflected-xss directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-reflected-xss <br />
        ///     Info: "(...) subsume the functionality provided by the proprietary X-XSS-Protection HTTP header (...)"
        /// </summary>
        public ReflectedXssKeyword ReflectedXss { get; set; }

        /// <summary>
        ///     Gets the report-uri directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-report-uri
        /// </summary>
        public CspUriReferenceList ReportUri { get; private set; }

        /// <summary>
        ///     Gets the sandbox directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-sandbox
        /// </summary>
        public CspSandboxTokenList Sandbox { get; private set; }

        /// <summary>
        ///     Gets the script-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-script-src <br />
        ///     Info: Hash and Nonce not implemented yet.
        /// </summary>
        public CspSourceList ScriptSrc { get; private set; }
        /// <summary>
        ///     Gets the style-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-style-src <br />
        ///     Info: Hash and Nonce not implemented yet.
        /// </summary>
        public CspSourceList StyleSrc { get; set; }

        /// <summary>
        ///     Creates the header values of all set directves.
        /// </summary>
        /// <returns>The content-security-policy header value.</returns>
        public string ToHeaderValue() {
            var values = new List<string>(16) {
                BuildDirectiveValue("base-uri", BaseUri),
                BuildDirectiveValue("child-src", ChildSrc),
                BuildDirectiveValue("connect-src", ConnectSrc),
                BuildDirectiveValue("default-src", DefaultSrc),
                BuildDirectiveValue("font-src", FontSrc),
                BuildDirectiveValue("form-action", FormAction),
                BuildDirectiveValue("frame-ancestors", FrameAncestors),
                BuildDirectiveValue("frame-src", FrameSrc),
                BuildDirectiveValue("img-src", ImgSrc),
                BuildDirectiveValue("media-src", MediaSrc),
                BuildDirectiveValue("object-src", ObjectSrc),
                BuildDirectiveValue("plugin-types", PluginTypes),
                BuildDirectiveValue("referrer", ReferrerDirectiveValueBuilder.Get(Referrer)),
                BuildDirectiveValue("reflected-xss", ReflectedXssDirectiveValueBuilder.Get(ReflectedXss)),
                BuildDirectiveValue("report-uri", ReportUri),
                BuildSandboxDirectiveValue(),
                BuildDirectiveValue("script-src", ScriptSrc),
                BuildDirectiveValue("style-src", StyleSrc)
            };
            var sb = new StringBuilder();
            for (int i = 0; i < values.Count; i++) {
                string value = values[i];
                if (value.IsNullOrWhiteSpace()) {
                    continue;
                }
                if (sb.Length > 0) {
                    sb.Append("; ");
                }
                sb.Append(value);
            }
            return sb.ToString();
        }
        private string BuildSandboxDirectiveValue() {
            if (Sandbox.IsEmpty) {
                return "sandbox";
            }
            return BuildDirectiveValue("sandbox", Sandbox);
        }

        private static string BuildDirectiveValue(string directiveName, IDirectiveValueBuilder sourceList) {
            return BuildDirectiveValue(directiveName, sourceList.ToDirectiveValue());
        }
        private static string BuildDirectiveValue(string directiveName, string directiveValue) {
            if (directiveValue.IsNullOrWhiteSpace()) {
                return "";
            }
            return "{0} {1}".FormatWith(directiveName, directiveValue);
        }
    }
}