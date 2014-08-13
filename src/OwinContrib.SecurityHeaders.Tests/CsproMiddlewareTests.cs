using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CsproMiddlewareTests {
        [Fact]
        public async Task When_adding_csp_middleware_a_response_should_serve_the_csp_header() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https:");
            config.ImgSrc.AddKeyword(SourceListKeyword.Self);
            HttpClient client = CsproClientHelper.Create(config);
            HttpResponseMessage response = await client.GetAsync("https://wwww.example.com");
            response.Cspro().Should().NotBeNullOrWhiteSpace();
        } 
    }

    internal static class CsproClientHelper {
        public static HttpClient Create(ContentSecurityPolicyConfiguration configuration) {
            return TestServer.Create(builder => {
                builder.UseOwin().ContentSecurityPolicyReportOnly(configuration);
                builder
                    .Use((context, next) => {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    });
            }).HttpClient;
        }
    }
}