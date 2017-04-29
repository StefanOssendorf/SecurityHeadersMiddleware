using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SecurityHeaders.Builders;
using SecurityHeaders.Tests.Infrastructure;
using Xunit;

namespace SecurityHeaders.Tests {
    public class ContentTypeOptionsMiddlewareTests {

        public class HeaderAlreadyPresent {
            [Fact]
            public async Task It_should_net_be_overwritten_with_set_configuration() {
                using(var client = CreateClient(b => b.IgnoreIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync("http://www.example.org");
                    response.XContentTypeOptions().Should().Be("test");
                }
            }

            [Fact]
            public async Task It_should_be_overwritten_with_nosniff() {
                using(var client = CreateClient(b => b.OverwriteHeaderIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync("http://www.example.org");
                    response.XContentTypeOptions().Should().Be("nosniff");
                }
            }
        }

        public class HeaderNotPresent {
            [Fact]
            public async Task Default_middleware_should_add_the_header() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync("http://www.example.org");
                    response.XContentTypeOptions().Should().Be("nosniff");
                }
            }
        }

        private static HttpClient CreateClient(Action<IFluentCtoSettingsBuilder> settingsBuilder = null, string headerValue = null) => TestHttpClientFactory.CreateCto(settingsBuilder, headerValue);
    }
}