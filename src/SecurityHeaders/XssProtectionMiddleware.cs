using System;

namespace SecurityHeaders {
    /// <summary>
    /// The middleware to apply the X-Xss-Protection header.
    /// </summary>
    public class XssProtectionMiddleware {

        /// <summary>
        /// /// <summary>
        /// The http-header name of the x-xss-protection header.
        /// </summary>
        /// </summary>
        public const string XXssProtectionHeaderName = "X-Xss-Protection";

        /// <summary>
        /// Applies the middleware on the context.
        /// </summary>
        /// <param name="context">The context. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="context"/> is <code>null</code>.</exception>
        public void ApplyHeader(IHttpContext context) {
            Guard.NotNull(context, nameof(context));
        }
    }

    /// <summary>
    /// Defines the Xss protection settings. <br/>
    /// This type is immutable.
    /// </summary>
    public class XssProtectionSettings {


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