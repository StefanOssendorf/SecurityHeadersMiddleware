using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using SecurityHeaders.Builders;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder builder) => builder.UseXssProtection(_ => { });

        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder builder, Action<IFluentXpSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new XpSettingsBuilder();
            builderAction(settingsBuilder);
            XssProtectionSettings settings = settingsBuilder.Build();
            var middleware = new XssProtectionMiddleware(settings);

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