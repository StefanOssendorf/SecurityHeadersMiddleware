using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore {
    /// <summary>
    /// IApplicationBuilder extension methods.
    /// </summary>
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder) => UseContentTypeOptions(builder, () => new ContentTypeOptionsSettings());

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <param name="getSettings">The action to set the settings.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder, Func<ContentTypeOptionsSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            var cto = new ContentTypeOptionsMiddleware(getSettings());
            builder.Use(async (context, func) => {
                context.Response.OnStarting(innerCtx => {
                    cto.ApplyHeader(innerCtx);
                    return Task.CompletedTask;
                }, context.AsInternalCtx());
                await func();
            });

            return builder;
        }
    }
}