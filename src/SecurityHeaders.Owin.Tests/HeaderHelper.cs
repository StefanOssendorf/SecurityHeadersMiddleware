using System.Linq;
using System.Net.Http;

namespace SecurityHeaders.Owin.Tests {
    internal static class HeaderHelper {
        public static string XFrameOptions(this HttpResponseMessage source) => source.Headers.GetValues(AntiClickjackingMiddleware.XFrameOptionsHeaderName).First();

        //public static IEnumerable<string> StsHeader(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.StrictTransportSecurity).Single().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim());
        //}

        public static string XContentTypeOptions(this HttpResponseMessage source) => source.Headers.GetValues(ContentTypeOptionsMiddleware.XContentTypeOptionsHeaderName).Single();

        //public static string XssProtection(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.XssProtection).First();
        //}

        //public static string Csp(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicy).First();
        //}

        //public static string Cspro(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicyReportOnly).First();
        //}
    }
}