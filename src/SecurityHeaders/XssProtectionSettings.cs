using System;

namespace SecurityHeaders {
    /// <summary>
    /// Defines the Xss protection settings. <br/>
    /// This type is immutable.
    /// </summary>
    public class XssProtectionSettings {

        /// <summary>
        /// Get how the middleware should handle this header.
        /// </summary>
        public HeaderControl HeaderHandling { get; }

        /// <summary>
        /// Get the value of the header to be set when applied.
        /// </summary>
        public XssProtectionHeaderValue HeaderValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XssProtectionSettings"/> class with default behavior.
        /// </summary>
        public XssProtectionSettings() : this(XssProtectionHeaderValue.EnabledAndBlock()) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XssProtectionSettings"/> class.
        /// </summary>
        /// <param name="headerValue">The header value which should be applied. Must not be <code>null</code>.</param>
        /// <param name="headerHandling">How the header should be handled.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="headerValue"/> is <code>null</code>.</exception>
        public XssProtectionSettings(XssProtectionHeaderValue headerValue, HeaderControl headerHandling = HeaderControl.OverwriteIfHeaderAlreadySet) {
            Guard.NotNull(headerValue, nameof(headerValue));
            Guard.MustBeDefined(headerHandling, nameof(headerHandling));
            HeaderHandling = headerHandling;
            HeaderValue = headerValue;
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