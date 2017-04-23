using System.Linq;
using System.Net.Http;

namespace SecurityHeaders.AspNetCore.Tests {
    internal static class HeaderHelper {
        public const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        public const string XFrameOptionsHeaderName = "X-Frame-Options";
        public const string XXssProtectionHeaderName = "X-Xss-Protection";

        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(XContentTypeOptionsHeaderName).Single();
        }

        public static string XFrameOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(XFrameOptionsHeaderName).Single();
        }

        public static string XssProtection(this HttpResponseMessage source) {
            return source.Headers.GetValues(XXssProtectionHeaderName).Single();
        }
    }
}