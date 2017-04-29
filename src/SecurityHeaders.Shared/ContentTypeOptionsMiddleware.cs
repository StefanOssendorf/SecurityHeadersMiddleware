using System;

namespace SecurityHeaders {
    /// <summary>
    /// The middleware to apply the content-type-options header.
    /// </summary>
    internal class ContentTypeOptionsMiddleware {
        /// <summary>
        /// The http-header name of the content-type-options header.
        /// </summary>
        public const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        
        /// <summary>
        /// The value of the content-type-options header.
        /// </summary>
        public const string XContentTypeOptionsHeaderValue = "nosniff";

        private readonly ContentTypeOptionsSettings mSettings;

        /// <summary>
        /// Initializes a new instance of <see cref="ContentTypeOptionsMiddleware"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        /// /// <exception cref="ArgumentNullException">When <paramref name="settings"/> is <code>null</code>.</exception>
        public ContentTypeOptionsMiddleware(ContentTypeOptionsSettings settings) {
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

            context.SetHeader(XContentTypeOptionsHeaderName, XContentTypeOptionsHeaderValue);
        }

        private bool HeaderShouldNotBeSet(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return false;
            }

            return headerExist(XContentTypeOptionsHeaderName) && mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }
}