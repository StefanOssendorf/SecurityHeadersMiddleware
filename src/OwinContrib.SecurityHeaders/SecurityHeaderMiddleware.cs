using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwinContrib.SecurityHeaders {
    public static class SecurityHeaderMiddleware {
        #region AntiClickjacking
        /// <summary>
        /// Adds the "X-Frame-Options" header with value DENY.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> AntiClickjackingHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(XFrameOption.Deny));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" header with given option.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="option">Chosen value.</param>
        /// <returns></returns>
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> AntiClickjackingHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, XFrameOption option) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(option));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM value.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="origins"></param>
        /// <returns></returns>
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> AntiClickjackingHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, params string[] origins) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(origins));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM &lt;request uri&gt;.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="origins"></param>
        /// <returns></returns>
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> AntiClickjackingHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, params Uri[] origins) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(origins));
            return builder;
        }
        #endregion
        #region XssProtection
        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block".
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> XssProtectionHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder) {
            return XssProtectionHeader(builder, false);
        }
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> XssProtectionHeader(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, bool disabled) {
            builder(XssProtectionHeaderMiddleware.XssProtectionHeader(disabled));
            return builder;
        }
        #endregion
        #region Strict Transport Security
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> StrictTransportSecurity(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder) {
            return StrictTransportSecurity(builder, new StrictTransportSecurityOptions());
        }
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> StrictTransportSecurity(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder, StrictTransportSecurityOptions options) {
            builder(StrictTransportSecurityHeaderMiddleware.StrictTransportSecurityHeader(options));
            return builder;
        }
        #endregion 
        #region Content Type Options
        public static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> ContentTypeOptions(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> builder) {
            builder(ContenTypeOptionsHeaderMiddleware.ContentTypeOptionsHeader());
            return builder;
        }
        #endregion
    }
}