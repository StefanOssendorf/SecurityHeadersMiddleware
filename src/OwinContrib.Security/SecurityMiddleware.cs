using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OwinContrib.Security {
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;
    using BuildFunc = Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>;
    
    public static class SecurityMiddleware {
        #region AntiClickjacking
        /// <summary>
        /// Adds the "X-Frame-Options" header with value DENY.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(XFrameOption.Deny));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" header with given option.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="option">Chosen value.</param>
        /// <returns></returns>
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, XFrameOption option) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(option));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM value.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="origins"></param>
        /// <returns></returns>
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, params string[] origins) {
            builder(AntiClickjackingMiddleware.AntiClickjackingHeader(origins));
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM &lt;request uri&gt;.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="origins"></param>
        /// <returns></returns>
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, params Uri[] origins) {
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
        public static BuildFunc XssProtectionHeader(this BuildFunc builder) {
            builder(XssProtectionHeaderMiddleware.XssProtectionHeader());
            return builder;
        }
        #endregion
    }
}