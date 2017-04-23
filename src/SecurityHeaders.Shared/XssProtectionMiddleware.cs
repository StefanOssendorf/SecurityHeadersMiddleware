using System;

namespace SecurityHeaders {
    /// <summary>
    /// The middleware to apply the X-Xss-Protection header.
    /// </summary>
    internal class XssProtectionMiddleware {
        private readonly XssProtectionSettings mSettings;

        /// <summary>
        /// The http-header name of the x-xss-protection header.
        /// </summary>
        public const string XXssProtectionHeaderName = "X-Xss-Protection";

        /// <summary>
        /// Initializes a new instance of <see cref="XssProtectionMiddleware"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="settings"/> is <code>null</code>.</exception>
        public XssProtectionMiddleware(XssProtectionSettings settings) {
            Guard.NotNull(settings, nameof(settings));
            mSettings = settings;
        }

        /// <summary>
        /// Applies the middleware on the context.
        /// </summary>
        /// <param name="context">The context. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="context"/> is <code>null</code>.</exception>
        public void ApplyHeader(IHttpContext context) {
            Guard.NotNull(context, nameof(context));

            if(HeaderShouldNotBeSet(context.HeaderExist)) {
                return;
            }

            context.OverrideHeader(XXssProtectionHeaderName, mSettings.HeaderValue);
        }

        private bool HeaderShouldNotBeSet(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return false;
            }

            return headerExist(XXssProtectionHeaderName) && mSettings.HeaderHandling == XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }
}