using System;
using Owin;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Owin.AppBuilder {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IAppBuilder UseXssProtection(this IAppBuilder builder) {
            Guard.NotNull(builder, nameof(builder));
            builder.UseOwin().XssProtection();
            return builder;
        }

        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IAppBuilder UseXssProtection(this IAppBuilder builder, Action<IFluentXpSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            builder.UseOwin().XssProtection(builderAction);
            return builder;
        }
    }
}