namespace SecurityHeaders {
    /// <summary>
    /// Defines the Content-Type-Options settings. <br/>
    /// This type is immutable.
    /// </summary>
    public class ContentTypeOptionsSettings {
        
        /// <summary>
        /// Gets or sets the handling of the headervalue.
        /// </summary>
        public HeaderControl HeaderHandling { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeOptionsSettings"/> class.
        /// </summary>
        /// <param name="headerHandling">How the header should be handled.</param>
        public ContentTypeOptionsSettings(HeaderControl headerHandling = HeaderControl.OverwriteIfHeaderAlreadySet) {
            HeaderHandling = headerHandling;
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