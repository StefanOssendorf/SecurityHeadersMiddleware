using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SecurityHeaders.AspNetCore.Tests {
    internal static class HeaderHelper {
        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(ContentTypeOptionsMiddleware.XContentTypeOptionsHeaderName).Single();
        }

        public static string XFrameOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(AntiClickjackingMiddleware.XFrameOptionsHeaderName).Single();
        }

        public static string XssProtection(this HttpResponseMessage source) {
            return source.Headers.GetValues(XssProtectionMiddleware.XXssProtectionHeaderName).Single();
        }
    }
}