using System;
using Owin;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Owin.AppBuilder {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with value "max-age=31536000; includeSubDomains" to the response. 
        /// (31536000 = 12 month)
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IAppBuilder UseStrictTransportSecurity(this IAppBuilder builder) {
            Guard.NotNull(builder, nameof(builder));
            builder.UseOwin().StrictTransportSecurity();
            return builder;
        }

        /// <summary>
        /// Adds the "Strict-Transport-Security" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IAppBuilder UseStrictTransportSecurity(this IAppBuilder builder, Action<IFluentStsMaxAgeSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            builder.UseOwin().StrictTransportSecurity(builderAction);

            return builder;
        }
    }
}
