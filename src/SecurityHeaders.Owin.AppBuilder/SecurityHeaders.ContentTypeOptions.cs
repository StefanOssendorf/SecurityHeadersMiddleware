using System;
using Owin;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Owin.AppBuilder {

    /// <summary>
    /// Provides the extension methods for <see cref="IAppBuilder"/>.
    /// </summary>
    public static partial class SecurityHeaders {
        
        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response. <br/>
        /// Possible already present header will be overwritten.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        public static IAppBuilder UseContentTypeOptions(this IAppBuilder builder) => builder.UseContentTypeOptions(_ => { });

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response depending on the settings.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="builderAction">The action to configure the settings.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builderAction"/> is null.</exception>
        public static IAppBuilder UseContentTypeOptions(this IAppBuilder builder, Action<IFluentCtoSettingsBuilder> builderAction) {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(builderAction, nameof(builderAction));

            builder.UseOwin().ContentTypeOptions(builderAction);
            return builder;
        }
    }
}
