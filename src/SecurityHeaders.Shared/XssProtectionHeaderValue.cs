using System;
using SecurityHeaders.Infrastructure;

namespace SecurityHeaders {
    /// <summary>
    /// The X-Xss-Protection header value.
    /// </summary>
    internal class XssProtectionHeaderValue {

        /// <summary>
        /// Gets the header value for the x-xss-protection.
        /// </summary>
        public string HeaderValue { get; }

        private XssProtectionHeaderValue(string headerValue) {
            HeaderValue = headerValue;
        }

        /// <summary>
        /// Creates the header value with "0" which means the filter is disabled.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XssProtectionHeaderValue Disabled() => Create("0");

        /// <summary>
        /// Creates the header value with "1" which means the filter is enabled.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XssProtectionHeaderValue Enabled() => Create("1");

        /// <summary>
        /// Creates the header value with "1; mode=block" which means the filter is enabled and the page will not be rendered.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XssProtectionHeaderValue EnabledAndBlock() => Create("1; mode=block");

        /// <summary>
        /// Creates the header value with "1; report=&lt;your url&gt;" which means the filter is enabled and a report will be send to the provided url. <br/>
        /// This reporting feature is a chromium only feature.
        /// </summary>
        /// <param name="url">The url where the report should be send to.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="url"/> is null.</exception>
        public static XssProtectionHeaderValue EnabledAndReport(Uri url) {
            Guard.NotNull(url, nameof(url));
            return Create($"1; report={Rfc6454Utility.SerializeOrigin(url)}");
        }

        private static XssProtectionHeaderValue Create(string headerValue) => new XssProtectionHeaderValue(headerValue);

        /// <summary>
        /// Extracts the header value for implicit usage on strings.
        /// </summary>
        /// <param name="headerValue">The <see cref="XssProtectionHeaderValue"/> instance where the value should be extracted.</param>
        public static implicit operator string(XssProtectionHeaderValue headerValue) {
            return headerValue.HeaderValue;
        }
    }
}