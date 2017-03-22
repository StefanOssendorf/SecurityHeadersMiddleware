using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Frame-Options" header with value "DENY" to the response.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseAntiClickjacking(this IApplicationBuilder builder) => UseAntiClickjacking(builder, () => new AntiClickjackingSettings());

        /// <summary>
        /// Adds the "X-Frame-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseAntiClickjacking(this IApplicationBuilder builder, Func<AntiClickjackingSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            var cto = new AntiClickjackingMiddleware(getSettings());
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