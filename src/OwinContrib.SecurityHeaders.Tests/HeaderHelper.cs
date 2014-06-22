using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders.Tests {
    internal static class HeaderHelper {
        public static string XFrameOptionsHeader(this HttpResponseMessage source) {
            return source.Headers.GetValues(HeaderConstants.XFrameOptions).First();
        }
        public static IEnumerable<string> StsHeader(this HttpResponseMessage source) {
            return source.Headers.GetValues(HeaderConstants.StrictTransportSecurity).Single().Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim());
        }
        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(HeaderConstants.XContentTypeOptions).Single();
        }

        public static string XssProtection(this HttpResponseMessage soruce) {
            return soruce.Headers.GetValues(HeaderConstants.XssProtection).First();
        }
    }
}