using System;

namespace SecurityHeaders.Core {
    /// <summary>
    /// Defines the Content-Type-Options settings.
    /// </summary>
    public class ContentTypeOptionsSettings {
        private HeaderControl mHeaderHandling;

        /// <summary>
        /// Gets or sets the handling of the headervalue.
        /// </summary>
        public HeaderControl HeaderHandling {
            get { return mHeaderHandling; }
            set {
                value.MustBeDefined(nameof(HeaderHandling));
                mHeaderHandling = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeOptionsSettings"/> class with <see cref="HeaderHandling"/> = <see cref="HeaderControl.OverwriteIfHeaderAlreadySet"/>
        /// </summary>
        public ContentTypeOptionsSettings() {
            HeaderHandling = HeaderControl.OverwriteIfHeaderAlreadySet;
        }

        /// <summary>
        /// Specifies the handling of the header.
        /// </summary>
        public enum HeaderControl {
            /// <summary>
            /// If the header is already set, it will be replaced with the configured value.
            /// </summary>
            OverwriteIfHeaderAlreadySet = 0,
            /// <summary>
            /// If the header is already set, it will not be overwritten and the configured value will be ignored.
            /// </summary>
            IgnoreIfHeaderAlreadySet = 1,
        }
    }

    internal class ContentTypeOptions {
        internal const string XContentTypeOptions = "X-Content-Type-Options";
        internal const string XContentTypeOptionsValue = "nosniff";
        private readonly ContentTypeOptionsSettings mSettings;

        public ContentTypeOptions(ContentTypeOptionsSettings settings) {
            mSettings = settings;
        }

        public void ApplyHeader(IHttpContext context) {
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
            actionToModifyHeader(XContentTypeOptions, XContentTypeOptionsValue);
        }

        private bool SetHeader(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headerExist(XContentTypeOptions);
        }
    }
}