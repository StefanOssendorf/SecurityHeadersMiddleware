using System;
using SecurityHeaders.Infrastructure;

namespace SecurityHeaders {
    /// <summary>
    /// The X-Xss-Protection header value.
    /// </summary>
    public class XXssProtectionHeaderValue {

        /// <summary>
        /// Gets the header value for the x-xss-protection.
        /// </summary>
        public string HeaderValue { get; }

        private XXssProtectionHeaderValue(string headerValue) {
            HeaderValue = headerValue;
        }

        /// <summary>
        /// Creates the header value with "0" which means the filter is disabled.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XXssProtectionHeaderValue Disabled() => Create("0");

        /// <summary>
        /// Creates the header value with "1" which means the filter is enabled.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XXssProtectionHeaderValue Enabled() => Create("1");

        /// <summary>
        /// Creates the header value with "1; mode=block" which means the filter is enabled and the page will not be rendered.
        /// </summary>
        /// <returns>The represented header value.</returns>
        public static XXssProtectionHeaderValue EnabledAndBlock() => Create("1; mode=block");

        /// <summary>
        /// Creates the header value with "1; report=&lt;your url&gt;" which means the filter is enabled and a report will be send to the provided url. <br/>
        /// This reporting feature is a chromium only feature.
        /// </summary>
        /// <param name="url">The url where the report should be send to.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="url"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="url"/> is empty or whitespace.</exception>
        /// <exception cref="FormatException">When <paramref name="url"/> is not a valid <see cref="Uri"/>.</exception>
        public static XXssProtectionHeaderValue EnabledAndReport(string url) {
            Guard.NotNullOrWhiteSpace(url, nameof(url));
            return EnabledAndReport(new Uri(url));
        }

        /// <summary>
        /// Creates the header value with "1; report=&lt;your url&gt;" which means the filter is enabled and a report will be send to the provided url. <br/>
        /// This reporting feature is a chromium only feature.
        /// </summary>
        /// <param name="url">The url where the report should be send to.</param>
        /// <returns>The represented header value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="url"/> is null.</exception>
        public static XXssProtectionHeaderValue EnabledAndReport(Uri url) {
            Guard.NotNull(url, nameof(url));
            return Create($"1; report={Rfc6454Utility.SerializeOrigin(url)}");
        }

        private static XXssProtectionHeaderValue Create(string headerValue) => new XXssProtectionHeaderValue(headerValue);
    }
}