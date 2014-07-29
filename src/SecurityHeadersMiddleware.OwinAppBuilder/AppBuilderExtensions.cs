using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware.OwinAppBuilder {
    using BuildFunc = Action<Func<IDictionary<string, object>,Func<Func<IDictionary<string, object>, Task>,Func<IDictionary<string, object>, Task>>>>;

    public static class AppBuilderExtensions {
        #region AntiClickjacking
        /// <summary>
        /// Adds the "X-Frame-Options" header with value DENY to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder AntiClickjackingHeader(this IAppBuilder builder) {
            return AntiClickjackingHeader(builder, XFrameOption.Deny);
        }
        /// <summary>
        /// Adds the "X-Frame-Options" header with the given option to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="option">The X-Frame option.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder AntiClickjackingHeader(this IAppBuilder builder, XFrameOption option) {
            builder.MustNotNull("builder");
            
            builder.UseOwin().AntiClickjackingHeader(option);
            
            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided to the response. Otherwise the request uri with ALLOW-FROM &lt;request uri&gt;.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="origins">The allowed uris.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder AntiClickjackingHeader(this IAppBuilder builder, params string[] origins) {
            builder.MustNotNull("builder");
            origins.MustNotNull("origins");
            origins.MustHaveAtLeastOneValue("origins");

            builder.UseOwin().AntiClickjackingHeader(origins);

            return builder;
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided to the response. Otherwise the request uri with ALLOW-FROM &lt;request uri&gt;.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="origins">The allowed uirs.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder AntiClickjackingHeader(this IAppBuilder builder, params Uri[] origins) {
            builder.MustNotNull("builder");
            origins.MustNotNull("origins");
            origins.MustHaveAtLeastOneValue("origins");

            builder.UseOwin().AntiClickjackingHeader(origins);

            return builder;
        }
        #endregion

        #region XssProtection
        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder XssProtectionHeader(this IAppBuilder builder) {
            return XssProtectionHeader(builder, false);
        }
        /// <summary>
        /// Adds the "X-Xss-Protection" header depending on <paramref name="disabled"/> to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="disabled">true to set the heade value to "0". false to set the header value to"1; mode=block".</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder XssProtectionHeader(this IAppBuilder builder, bool disabled) {
            builder.MustNotNull("builder");

            builder.UseOwin().XssProtectionHeader(disabled);
            
            return builder;
        }
        #endregion

        #region Strict Transport Security
        /// <summary>
        /// Adds the "Strict-Transport-Security" (STS) header to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder StrictTransportSecurity(this IAppBuilder builder) {
            return StrictTransportSecurity(builder, new StrictTransportSecurityOptions());
        }
        /// <summary>
        /// Adds the "Strict-Transport-Security" (STS) header with the given option to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="options">The Strict-Transport-Security options.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder StrictTransportSecurity(this IAppBuilder builder, StrictTransportSecurityOptions options) {
            builder.MustNotNull("builder");
            options.MustNotNull("options");

            builder.UseOwin().StrictTransportSecurity(options);

            return builder;
        }
        #endregion

        #region Content Type Options
        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder) {
            builder.MustNotNull("builder");

            builder.UseOwin().ContentTypeOptions();
            
            return builder;
        }
        #endregion

        #region Content Security Policy
        /// <summary>
        /// Adds the "Content-Security-Policy" header with the given configuration to the response.
        /// </summary>
        /// <param name="builder">The IAppBuilder instance.</param>
        /// <param name="configuration">The Content-Security-Policy configuration.</param>
        /// <returns>The IAppBuilder instance.</returns>
        public static IAppBuilder ContentSecurityPolicy(this IAppBuilder builder, ContentSecurityPolicyConfiguration configuration) {
            builder.MustNotNull("builder");

            builder.UseOwin().ContentSecurityPolicy(configuration);
            return builder;
        }
        #endregion


        internal static BuildFunc UseOwin(this IAppBuilder builder) {
            return middleware => builder.Use(middleware(builder.Properties));
        }
    }
}