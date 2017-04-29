using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using SecurityHeaders.Builders;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with value "max-age=31536000; includeSubDomains" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IApplicationBuilder UseStrictTransportSecurity(this IApplicationBuilder builder) => builder.UseStrictTransportSecurity(_ => { });

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IApplicationBuilder UseStrictTransportSecurity(this IApplicationBuilder builder, Action<IFluentStsMaxAgeSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new StsSettingsBuilder();
            builderAction(settingsBuilder);
            StrictTransportSecuritySettings settings = settingsBuilder.Build();
            var middleware = new StrictTransportSecurityMiddleware(settings);

            builder.Use(async (context, next) => {

                var beforeResult = middleware.BeforeNext(context.AsInternalCtx());
                if(beforeResult.EndPipeline) {
                    return;
                }

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
