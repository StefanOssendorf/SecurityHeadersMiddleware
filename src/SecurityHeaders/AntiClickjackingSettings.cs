namespace SecurityHeaders {
    /// <summary>
    /// Defines the Anticlickjacking settings.
    /// </summary>
    public class AntiClickjackingSettings {

        /// <summary>
        /// Gets how the middleware should handle this header.
        /// </summary>
        public HeaderControl HeaderHandling { get; }

        /// <summary>
        /// Gets the value of the header to set when set.
        /// </summary>
        public XFrameOptionHeaderValue HeaderValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AntiClickjackingSettings"/> class with default behavior.
        /// </summary>
        public AntiClickjackingSettings() {
            HeaderHandling = HeaderControl.OverwriteIfHeaderAlreadySet;
            HeaderValue = XFrameOptionHeaderValue.Deny();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AntiClickjackingSettings"/> class.
        /// </summary>
        /// <param name="headerValue">The header value which should be applied.</param>
        /// <param name="headerHandling">How the header should be handled.</param>
        public AntiClickjackingSettings(XFrameOptionHeaderValue headerValue, HeaderControl headerHandling = HeaderControl.OverwriteIfHeaderAlreadySet) {
            Guard.NotNull(headerValue, nameof(headerValue));
            HeaderValue = headerValue;
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
