using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityHeaders.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    /// <summary>
    ///     OWIN extension methods.
    /// </summary>
    public static partial class SecurityHeaders {

        /// <summary>
        ///     Adds the "X-Content-Type-Options" header with value "nosniff" to the response. <br/>
        ///     Possible already present header will be overridden.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException">builder is null.</exception>
        public static BuildFunc ContentTypeOptions(this BuildFunc builder) {
            return ContentTypeOptions(builder, cs => { });
        }

        /// <summary>
        ///     Adds the "X-Content-Type-Options" header with value "nosniff" to the response. />.
        /// </summary>
        /// <param name="builder">The OWIN builder instance.</param>
        /// <param name="configureSettings">The action to set the settings.</param>
        /// <returns>The OWIN builder instance.</returns>
        /// <exception cref="ArgumentNullException">builder is null.</exception>
        /// <exception cref="ArgumentNullException">configureSettings is null.</exception>
        public static BuildFunc ContentTypeOptions(this BuildFunc builder, Action<ContentTypeOptionsSettings> configureSettings) {
            builder.MustNotNull(nameof(builder));
            configureSettings.MustNotNull(nameof(configureSettings));

            var settings = new ContentTypeOptionsSettings();
            configureSettings(settings);

            var middleware = new ContentTypeOptions(settings);
            builder(_ =>
                next =>
                    env => {
                        var ctx = env.AsOwinContext();
                        ctx.Response.OnSendingHeaders(ctx2 => middleware.ApplyHeader(ctx2), ctx.AsInternalCtx());
                        return next(env);
                    }
            );

            return builder;
        }
    }
}