using System;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {
    public static partial class SecurityHeaders {

        /// <summary>
        /// Adds the "X-Frame-Options" header with value "DENY" to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IAppBuilder UseAntiClickjacking(this IAppBuilder builder) => UseAntiClickjacking(builder, () => new AntiClickjackingSettings());

        /// <summary>
        /// Adds the "X-Frame-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="getSettings"/> is null.</exception>
        public static IAppBuilder UseAntiClickjacking(this IAppBuilder builder, Func<AntiClickjackingSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            builder.UseOwin().UseAntiClickjacking(getSettings);
            return builder;
        }
    }
}