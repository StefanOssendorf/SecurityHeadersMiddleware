using System;

namespace SecurityHeaders {
    /// <summary>
    /// Defines the Strict-Transport-Security settings. <br/>
    /// This type is immutable.
    /// </summary>
    internal class StrictTransportSecuritySettings {

        /// <summary>
        /// Get how the middleware should handle this header.
        /// </summary>
        public HeaderControl HeaderHandling { get; }

        /// <summary>
        /// Get if the request should be permanently redirected (301) to a secure url.
        /// </summary>
        public bool Redirect { get; }

        /// <summary>
        /// Get the value of the header to be set when applied.
        /// </summary>
        public StrictTransportSecurityHeaderValue HeaderValue { get; }

        public Func<Uri, Uri> RedirectBuilder { get; }

        internal StrictTransportSecuritySettings(StrictTransportSecurityHeaderValue headerValue, HeaderControl headerHandling, bool redirect, Func<Uri, Uri> redirectBuilder) {
            HeaderValue = headerValue;
            HeaderHandling = headerHandling;
            Redirect = redirect;
            RedirectBuilder = redirectBuilder;
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
