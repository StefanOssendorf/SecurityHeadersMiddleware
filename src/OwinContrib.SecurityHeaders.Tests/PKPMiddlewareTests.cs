using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Owin;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.OwinAppBuilder;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class PkpMiddlewareTests {
        [Fact]
        public async Task When_pkp_is_configured_and_request_is_made_over_http_it_should_not_set_the_header() {
            var config = new PublicKeyPinningConfiguration {
                MaxAge = 1,
                IncludeSubDomains = true
            };
            config.AddPin(1, "krznbf", PinToken.Sha256);
            var client = PkpClientHelper.Create(config);
            var response = await client.GetAsync("http://wwww.example.org");
            response.Headers.Should().NotContain(HeaderConstants.PublicKeyPinning);
        }

        internal class PkpClientHelper {
            public static HttpClient Create() {
                return Create(new PublicKeyPinningConfiguration());
            }

            public static HttpClient Create(PublicKeyPinningConfiguration configuration) {
                return TestServer.Create(builder => {
                    builder.UseOwin().PublicKeyPinning(configuration);
                    builder.Use((context, next) => {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    });
                }).HttpClient;
            }
        }
    }
}