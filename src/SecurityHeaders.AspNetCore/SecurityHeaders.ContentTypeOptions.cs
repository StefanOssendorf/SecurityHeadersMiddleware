using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using SecurityHeaders.Builders;

namespace SecurityHeaders.AspNetCore {
    
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response. <br/>
        /// Possible already present header will be overwritten.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder) => builder.UseContentTypeOptions(_ => { });

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response depending on the settings.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder, Action<IFluentCtoSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new CtoSettingsBuilder();
            builderAction(settingsBuilder);
            var settings = settingsBuilder.Build();
            var middleware = new ContentTypeOptionsMiddleware(settings);
            
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