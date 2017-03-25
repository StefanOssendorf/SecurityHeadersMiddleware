using System;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {

    /// <summary>
    /// Provides the extension methods for <see cref="IAppBuilder"/>.
    /// </summary>
    public static partial class SecurityHeaders {
        /// <summary>
        /// Adds the "X-Content-Type-Options" Header with default settings. to the response with <see cref="ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder) {
            Guard.NotNull(builder, nameof(builder));
            return ContentTypeOptions(builder, () => new ContentTypeOptionsSettings());
        }

        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="getSettings">The func to get the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptions(this IAppBuilder builder, Func<ContentTypeOptionsSettings> getSettings) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(getSettings, nameof(getSettings));

            builder.UseOwin().ContentTypeOptions(getSettings);
            return builder;
        }
    }
}
