using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public static BuildFunc XssProtection(this BuildFunc builder) => XssProtection(builder, () => new XssProtectionSettings());

        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="getSettings"/> is null.</exception>
        public static BuildFunc XssProtection(this BuildFunc builder, Func<XssProtectionSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            var middleware = new XssProtectionMiddleware(getSettings());
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