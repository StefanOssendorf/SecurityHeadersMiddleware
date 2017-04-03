using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityHeaders.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Frame-Options" header with value "DENY" to the response.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static BuildFunc AntiClickjacking(this BuildFunc builder) => AntiClickjacking(builder, () => new AntiClickjackingSettings());

        /// <summary>
        /// Adds the "X-Frame-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="getSettings"/> is null.</exception>
        public static BuildFunc AntiClickjacking(this BuildFunc builder, Func<AntiClickjackingSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            var middleware = new AntiClickjackingMiddleware(getSettings());
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