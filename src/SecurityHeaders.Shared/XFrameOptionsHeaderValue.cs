using System;
using SecurityHeaders.Infrastructure;

namespace SecurityHeaders {
    /// <summary>
    /// The X-Frame-Option header value.
    /// </summary>
    internal class XFrameOptionsHeaderValue {

        /// <summary>
        /// Gets the header value for the x-frame-option.
        /// </summary>
        public string HeaderValue { get; }

        private XFrameOptionsHeaderValue(string headerValue) {
            Guard.NotNullOrWhiteSpace(headerValue, nameof(headerValue));
            HeaderValue = headerValue;
        }

        /// <summary>
        /// Creates a header value with DENY.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XFrameOptionsHeaderValue Deny() => Create("DENY");

        /// <summary>
        /// Creates a header value with SAMEORIGIN.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XFrameOptionsHeaderValue SameOrigin() => Create("SAMEORIGIN");

        /// <summary>
        /// Creates a header value of ALLOW-FROM with the provided <paramref name="origin"/>.
        /// </summary>
        /// <param name="origin">The origin which should be allowed.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="origin"/> is null.</exception>
        public static XFrameOptionsHeaderValue AllowFrom(Uri origin) {
            Guard.NotNull(origin, nameof(origin));
            return Create($"ALLOW-FROM {Rfc6454Utility.SerializeOrigin(origin)}");
        }

        private static XFrameOptionsHeaderValue Create(string value) => new XFrameOptionsHeaderValue(value);

        /// <summary>
        /// Extracts the header value for implicit usage on strings.
        /// </summary>
        /// <param name="headerValue">The <see cref="XFrameOptionsHeaderValue"/> instance where the value should be extracted.</param>
        public static implicit operator string(XFrameOptionsHeaderValue headerValue) {
            return headerValue.HeaderValue;
        }
    }
}
