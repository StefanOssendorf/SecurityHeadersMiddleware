using System.Linq;
using System.Net.Http;

namespace SecurityHeaders.AspNetCore.Tests {
    internal static class HeaderHelper {
        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(ContentTypeOptions.XContentTypeOptions).Single();
        }
    }
}