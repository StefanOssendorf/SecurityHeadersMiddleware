namespace SecurityHeaders {
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
}