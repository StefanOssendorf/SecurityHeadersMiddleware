using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeaders.Builders;
using SecurityHeaders.Owin.Infrastructure;

namespace SecurityHeaders.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static BuildFunc XssProtection(this BuildFunc builder) => builder.XssProtection(_ => { });

        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static BuildFunc XssProtection(this BuildFunc builder, Action<IFluentXpSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            var settingsBuilder = new XpSettingsBuilder();
            builderAction(settingsBuilder);
            XssProtectionSettings settings = settingsBuilder.Build();
            var middleware = new XssProtectionMiddleware(settings);

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