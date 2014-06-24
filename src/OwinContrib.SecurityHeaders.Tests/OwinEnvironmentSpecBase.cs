using System.Net.Http;

namespace SecurityHeadersMiddleware.Tests {
    public abstract class OwinEnvironmentSpecBase {
        protected static HttpClient Client;
        protected static HttpResponseMessage Response;
    }
}