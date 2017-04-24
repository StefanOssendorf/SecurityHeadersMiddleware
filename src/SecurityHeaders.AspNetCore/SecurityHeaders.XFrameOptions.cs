using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using SecurityHeaders.Builders;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {

       /// <summary>
        /// Adds the "X-Frame-Options" header with value "DENY" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IApplicationBuilder UseXFrameOptions(this IApplicationBuilder builder) => builder.UseXFrameOptions(_ => { });

       /// <summary>
        /// Adds the "X-Frame-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IApplicationBuilder UseXFrameOptions(this IApplicationBuilder builder, Action<IFluentXfoSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new XfoSettingsBuilder();
            builderAction(settingsBuilder);
            XFrameOptionsSettings settings = settingsBuilder.Build();
            var middleware = new XFrameOptionsMiddleware(settings);

            builder.Use(async (context, next) => {
                context.Response.OnStarting(innerCtx => {
                    middleware.ApplyHeader(innerCtx);
                    return Task.CompletedTask;
                }, context.AsInternalCtx());
                await next.Invoke();
            });

            return builder;
        }
    }
}