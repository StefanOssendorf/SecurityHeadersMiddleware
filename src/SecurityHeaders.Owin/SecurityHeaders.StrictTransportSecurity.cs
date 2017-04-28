using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;


    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with value "max-age=31536000; includeSubDomains" to the response. 
        /// (31536000 = 12 month)
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static BuildFunc StrictTransportSecurity(this BuildFunc builder) => builder.ContentTypeOptions(_ => { });

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with the configured settings.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static BuildFunc StrictTransportSecurity(this BuildFunc builder, Action<IFluentStsMaxAgeSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            //dynamic settingsBuilder = null;
            //builderAction(settingsBuilder);
            //var settings = settingsBuilder.Build();
            //var middleware = new StrictTransportSecurityMiddleware(settings);

            //builder(_ => next =>
            //    env => {
            //        var ctx = env.AsOwinContext();
            //        ctx.Response.OnSendingHeaders(ctx2 => middleware.ApplyHeader(ctx2), ctx.AsInternalCtx());
            //        return next(env);
            //    });

            return builder;
        }
    }
}
