using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Xunit;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Tests {
    public class CspMiddlewareTests {
        [Fact]
        public async Task When_adding_csp_middleware_a_response_should_serve_the_csp_header() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https:");
            config.ImgSrc.AddKeyword(CspKeyword.Self);
            var client = CspClientHelper.Create(config);
            var response = await client.GetAsync("https://wwww.example.com");

            response.Csp().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task When_adding_csp_middleware_the_response_should_contain_the_expected_csp_directives() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https:");
            config.ImgSrc.AddKeyword(CspKeyword.Self);
            var client = CspClientHelper.Create(config);
            var response = await client.GetAsync("https://wwww.example.com");
            var headerValue = response.Csp();

            var values = headerValue.Split(new[] {";"}, StringSplitOptions.None).Select(i => i.Trim()).ToList();
            values.Count.Should().Be(2);
            values.Should().Contain(i => i.Equals("img-src 'self'"));
            values.Should().Contain(i => i.Equals("script-src https:"));
        }
    }

    internal static class CspClientHelper {
        public static HttpClient Create(ContentSecurityPolicyConfiguration configuration) {
            return TestServer.Create(builder => {
                builder.UseOwin().ContentSecurityPolicy(configuration);
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