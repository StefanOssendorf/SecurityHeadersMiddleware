using System;

namespace SecurityHeaders {

    /// <summary>
    ///  The middleware to apply the X-Frame-Options header.
    /// </summary>
    internal class XFrameOptionsMiddleware {

        private readonly XFrameOptionsSettings mSettings;

        /// <summary>
        /// The http-header name of the x-frame-options header.
        /// </summary>
        public const string XFrameOptionsHeaderName = "X-Frame-Options";

        /// <summary>
        /// Initializes a new instance of <see cref="XFrameOptionsMiddleware"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="settings"/> is <code>null</code>.</exception>
        public XFrameOptionsMiddleware(XFrameOptionsSettings settings) {
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

            context.OverrideHeader(XFrameOptionsHeaderName, mSettings.HeaderValue);
        }

        private bool HeaderShouldNotBeSet(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == XFrameOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return false;
            }

            return headerExist(XFrameOptionsHeaderName) && mSettings.HeaderHandling == XFrameOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }
}
