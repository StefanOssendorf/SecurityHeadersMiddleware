using System;
using System.Collections.Generic;
using System.Linq;
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
            FrameAncestors = new CspAncestorSourceList();
            FrameSrc = new CspSourceList();
            ImgSrc = new CspSourceList();
            MediaSrc = new CspSourceList();
            ObjectSrc = new CspSourceList();
            ScriptSrc = new CspSourceList();
            StyleSrc = new CspSourceList();
            PluginTypes = new CspMediaTypeList();
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
        /// <summary>
        ///     Gets the frame-ancestors directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-frame-ancestors <br />
        ///     Info: According to the spec this directive replaces the X-Frame-Options header.
        /// </summary>
        public CspAncestorSourceList FrameAncestors { get; private set; }
        /// <summary>
        ///     Gets the frame-src directive source-list.<br />
        ///     See http://www.w3.org/TR/CSP2/#directive-frame-src
        /// </summary>
        [Obsolete("\"The frame-src directive is deprecated. Authors who wish to govern nested browsing contexts SHOULD use the child-src directive instead.\" See http://www.w3.org/TR/CSP/#directive-frame-src")]
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
                BuildDirectiveValue("report-uri", ReportUri),
                BuildSandboxDirectiveValue(),
                BuildDirectiveValue("script-src", ScriptSrc),
                BuildDirectiveValue("style-src", StyleSrc)
            };
            return string.Join("; ", values.Where(s => !s.IsNullOrWhiteSpace()));
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