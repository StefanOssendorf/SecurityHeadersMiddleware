using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SecurityHeaders.Builders;
using SecurityHeaders.Tests.Infrastructure;
using Xunit;

namespace SecurityHeaders.Tests {
    public class StrictTransportSecurityMiddlewareTests {
        private const string HttpExampleOrgUrl = "http://www.example.org";
        private const string HttpsExampleOrgUrl = "https://www.example.org";

        public class HeaderAlreadyPresent {
            [Fact]
            public async Task When_header_should_be_ignoredIfPresent_it_should_not_modify_the_header() {
                using(var client = CreateClient(b => b.WithMaxAge(0).IncludeSubdomains().WithoutPreload().IgnoreIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(HttpsExampleOrgUrl);
                    var headerValues = response.StrictTransportSecurity();
                    headerValues.Count.Should().Be(1);
                    headerValues[0].Should().Be("test");
                }
            }

            [Fact]
            public async Task When_header_should_be_overwritten_it_should_be_the_expected_value() {
                using(var client = CreateClient(b => b.WithMaxAge(0).IncludeSubdomains().WithPreload().OverwriteHeaderIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(HttpsExampleOrgUrl);
                    var headerValues = response.StrictTransportSecurity();
                    headerValues.Count.Should().Be(3);
                    headerValues[0].Should().Be("max-age=0");
                    headerValues[1].Should().Be("includeSubDomains");
                    headerValues[2].Should().Be("preload");
                }
            }
        }

        public class HeaderNotPresent {
            [Fact]
            public async Task Default_middleware_should_add_the_header_on_https() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync(HttpsExampleOrgUrl);
                    var headerValues = response.StrictTransportSecurity();
                    headerValues.Count.Should().Be(2);
                    headerValues[0].Should().Be("max-age=31536000");
                    headerValues[1].Should().Be("includeSubDomains");
                }
            }

            [Fact]
            public async Task Default_middleware_should_not_redirect_from_http_to_https() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync(HttpExampleOrgUrl);
                    response.Headers.Location.Should().BeNull();
                    response.StatusCode.Should().NotBe(HttpStatusCode.MovedPermanently);
                }
            }

            [Fact]
            public async Task When_middleware_should_redirect_on_http_it_should_return_httpStatusCode_301_and_an_https_url() {
                using(var client = CreateClient(b => b.WithMaxAge(10).IncludeSubdomains().WithPreload().RedirectUnsecureToSecureRequests().OverwriteHeaderIfHeaderIsPresent())) {
                    var response = await client.GetHeaderAsync(HttpExampleOrgUrl);
                    response.Headers.Location.Should().NotBeNull();
                    response.StatusCode.Should().Be(HttpStatusCode.MovedPermanently);
                    response.Headers.Location.Scheme.Should().BeEquivalentTo("https");
                }
            }
        }

        private static HttpClient CreateClient(Action<IFluentStsMaxAgeSettingsBuilder> settingsBuilder = null, string headerValue = null) => TestHttpClientFactory.CreateSts(settingsBuilder, headerValue);
    }
}