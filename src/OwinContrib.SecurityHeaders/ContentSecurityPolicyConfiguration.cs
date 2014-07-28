using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

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
        // TODO: media-type-list
        //public CspSourceList PluginTypes { get; private set; }

        /// <summary>
        /// Gets the referrer directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP11/#directive-referrer
        /// </summary>
        // TODO: referrer keywords
        //public CspSourceList Referrer { get; private set; }

        /// <summary>
        /// Gets the reflected-xss directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-reflected-xss <br/>
        /// Info: "(...) subsume the functionality provided by the proprietary X-XSS-Protection HTTP header (...)"
        /// </summary>
        // TODO: relfected-xss keywords
        //public CspSourceList ReflectedXss { get; private set; }

        /// <summary>
        /// Gets the report-uri directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-report-uri
        /// </summary>
        // TODO: URI from RFC3986
        //public CspSourceList ReportUri { get; private set; }

        /// <summary>
        /// Gets the sandbox directive source-list.<br/>
        /// See http://www.w3.org/TR/CSP2/#directive-sandbox
        /// </summary>
        // TODO: Sandbox-token (RFC7230)
        //public CspSourceList Sandbox { get; private set; }

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
        }

        public string ToHeaderValue() {
            var values = new List<string>(16);
            values.Add(BuildDirectiveValue("base-uri", BaseUri));
            values.Add(BuildDirectiveValue("child-src", ChildSrc));
            values.Add(BuildDirectiveValue("connect-src", ConnectSrc));
            values.Add(BuildDirectiveValue("default-src", DefaultSrc));
            values.Add(BuildDirectiveValue("font-src", FontSrc));
            values.Add(BuildDirectiveValue("form-action", FormAction));
            values.Add(BuildDirectiveValue("frame-ancestors", FrameAncestors));
            values.Add(BuildDirectiveValue("frame-src", FrameSrc));
            values.Add(BuildDirectiveValue("img-src", ImgSrc));
            values.Add(BuildDirectiveValue("media-src", MediaSrc));
            values.Add(BuildDirectiveValue("object-src", ObjectSrc));
            //values.Add(BuildDirectiveValue("plugin-types", PluginTypes));
            //values.Add(BuildDirectiveValue("referrer", Referrer));
            //values.Add(BuildDirectiveValue("reflected-xss", ReflectedXss));
            //values.Add(BuildDirectiveValue("report-uri", ReportUri));
            //values.Add(BuildDirectiveValue("sandbox", Sandbox));
            values.Add(BuildDirectiveValue("script-src", ScriptSrc));
            values.Add(BuildDirectiveValue("style-src", StyleSrc));

            var sb = new StringBuilder();
            for (int i = 0; i < values.Count; i++) {
                var value = values[i];
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

        private string BuildDirectiveValue(string directiveName, CspSourceList sourceList) {
            var directiveValue = sourceList.ToDirectiveValue();
            if (directiveValue.IsNullOrWhiteSpace()) {
                return "";
            }
            return "{0} {1}".FormatWith(directiveName, directiveValue);
        }
    }
}