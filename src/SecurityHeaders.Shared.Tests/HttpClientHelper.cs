using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SecurityHeaders.Tests {
    internal static class HttpClientHelper {
        public const string XContentTypeOptionsHeaderName = "X-Content-Type-Options";
        public const string XFrameOptionsHeaderName = "X-Frame-Options";
        public const string XXssProtectionHeaderName = "X-Xss-Protection";
        public const string StrictTransportSecurityHeaderName = "Strict-Transport-Security";

        public static string XFrameOptions(this HttpResponseMessage source) => source.Headers.GetValues(XFrameOptionsHeaderName).First();

        public static IList<string> StrictTransportSecurity(this HttpResponseMessage source) => source.Headers.GetValues(StrictTransportSecurityHeaderName).Single().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim()).ToList();

        public static string XContentTypeOptions(this HttpResponseMessage source) => source.Headers.GetValues(XContentTypeOptionsHeaderName).Single();

        public static string XssProtection(this HttpResponseMessage source) => source.Headers.GetValues(XXssProtectionHeaderName).Single();

        //public static string Csp(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicy).First();
        //}

        //public static string Cspro(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicyReportOnly).First();
        //}

        public static Task<HttpResponseMessage> GetHeaderAsync(this HttpClient client, string url) => client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
    }
}
