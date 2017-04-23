using System;

namespace SecurityHeaders.Builders {
    /// <summary>
    /// Fluent interface to configure the X-Frame-Options settings.
    /// </summary>
    public interface IFluentXfoSettingsBuilder : IHideObjectMethods {

        /// <summary>
        /// Set the header value to "DENY".
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder Deny();

        /// <summary>
        /// Set the header value to "SAMEORIGIN".
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder SameOrigin();

        /// <summary>
        /// Set the header value to "ALLOW-FROM" with the provided url.
        /// </summary>
        /// <param name="url">The url to allow from.</param>
        /// <returns>Next step in settings configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="url"/> is <code>null</code>.</exception>
        IFluentHeaderHandlingBuilder AllowFrom(Uri url);
    }
}