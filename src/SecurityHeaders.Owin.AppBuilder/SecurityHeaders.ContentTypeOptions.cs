using System;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {
    public static partial class SecurityHeaders {
        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response with <see cref="ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder) {
            builder.MustNotNull(nameof(builder));
            return ContentTypeOptions(builder, settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
        }

        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="configureSettings">Action to configure the settings-object.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder, Action<ContentTypeOptionsSettings> configureSettings) {
            builder.MustNotNull(nameof(builder));
            configureSettings.MustNotNull(nameof(configureSettings));

            builder.UseOwin().ContentTypeOptions(configureSettings);
            return builder;
        }
    }
}
