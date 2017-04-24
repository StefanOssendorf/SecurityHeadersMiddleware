using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeaders.Builders;
using SecurityHeaders.Owin.Infrastructure;

namespace SecurityHeaders.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response. <br/>
        /// Possible already present header will be overwritten.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static BuildFunc ContentTypeOptions(this BuildFunc builder) => builder.ContentTypeOptions(_ => {});

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response depending on the settings.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static BuildFunc ContentTypeOptions(this BuildFunc builder, Action<IFluentCtoSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new CtoSettingsBuilder();
            builderAction(settingsBuilder);
            var settings = settingsBuilder.Build();
            var middleware = new ContentTypeOptionsMiddleware(settings);

            builder(_ => next =>
                env => {
                    var ctx = env.AsOwinContext();
                    ctx.Response.OnSendingHeaders(ctx2 => middleware.ApplyHeader(ctx2), ctx.AsInternalCtx());
                    return next(env);
                });

            return builder;
        }
    }
}