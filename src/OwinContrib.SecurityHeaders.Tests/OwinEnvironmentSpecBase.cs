using System.Net.Http;

namespace OwinContrib.SecurityHeaders.Tests {
    public abstract class OwinEnvironmentSpecBase {
        protected static HttpClient Client;
        protected static HttpResponseMessage Response;
    }
}