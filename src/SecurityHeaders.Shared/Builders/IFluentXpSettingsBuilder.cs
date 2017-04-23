using System;

namespace SecurityHeaders.Builders {
    /// <summary>
    /// Fluent interface to configure the X-Xss-Protection settings.
    /// </summary>
    public interface IFluentXpSettingsBuilder : IHideObjectMethods {

        /// <summary>
        /// Set the header value to "0".
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder Disabled();

        /// <summary>
        /// Set the header value to "1".
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder Enabled();

        /// <summary>
        /// Set the header value to "1; mode=block".
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder EnabledAndBlock();

        /// <summary>
        /// Set the header value to "1; report=&lt;your <paramref name="url"/>&gt;". <br/>
        /// This reporting feature is a chromium only feature.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="url"/> is <code>null</code>.</exception>
        IFluentHeaderHandlingBuilder EnabledAndReport(Uri url);
    }
}