using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder builder) => UseXssProtection(builder, () => new XssProtectionSettings());

        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder builder, Func<XssProtectionSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            var cto = new XssProtectionMiddleware(getSettings());
            builder.Use(async (context, next) => {
                context.Response.OnStarting(innerCtx => {
                    cto.ApplyHeader(innerCtx);
                    return Task.CompletedTask;
                }, context.AsInternalCtx());
                await next.Invoke();
            });

            return builder;
        }
    }
}