using System;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {
    public static partial class SecurityHeaders {
        /// <summary>
        /// Adds the "X-Xss-Protection" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="getSettings"/> is null.</exception>
        public static IAppBuilder UseXssProtection(this IAppBuilder builder, Func<XssProtectionSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            builder.UseOwin().XssProtection(getSettings);
            return builder;
        }

        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IAppBuilder UseXssProtection(this IAppBuilder builder) => UseXssProtection(builder, () => new XssProtectionSettings());
    }
}