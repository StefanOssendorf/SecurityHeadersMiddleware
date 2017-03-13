using System;

namespace SecurityHeaders {
    /// <summary>
    /// The middleware to apply the content-type-options header.
    /// </summary>
    public class ContentTypeOptions {
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
        /// Initializes a new instance of <see cref="ContentTypeOptions"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        public ContentTypeOptions(ContentTypeOptionsSettings settings) {
            Guard.NotNull(settings, nameof(settings));
            mSettings = settings;
        }

        /// <summary>
        /// Applies the middleware on the context.
        /// </summary>
        /// <param name="context">The context. Must not be <code>null</code>.</param>
        public void ApplyHeader(IHttpContext context) {
            Guard.NotNull(context, nameof(context));

            if(!SetHeader(context.HeaderExist)) {
                return;
            }

            Action<string, string> actionToModifyHeader;
            switch(mSettings.HeaderHandling) {
                case ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet:
                    actionToModifyHeader = context.OverrideHeader;
                    break;
                case ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet:
                    actionToModifyHeader = context.AppendToHeader;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown enum-value '{mSettings.HeaderHandling}' of enum ${typeof(ContentTypeOptionsSettings.HeaderControl).FullName}");
            }
            actionToModifyHeader(XContentTypeOptionsHeaderName, XContentTypeOptionsHeaderValue);
        }

        private bool SetHeader(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headerExist(XContentTypeOptionsHeaderName);
        }
    }
}