using System;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {

    /// <summary>
    /// Provides the extension methods for <see cref="IAppBuilder"/>.
    /// </summary>
    public static partial class SecurityHeaders {
        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response with <see cref="ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder) {
            Guard.NotNull(builder, nameof(builder));
            return ContentTypeOptions(builder, settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
        }

        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="configureSettings">Action to configure the settings-object.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder, Action<ContentTypeOptionsSettings> configureSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(configureSettings, nameof(configureSettings));

            builder.UseOwin().ContentTypeOptions(configureSettings);
            return builder;
        }
    }
}
