using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SecurityHeaders.Builders;
using SecurityHeaders.Tests.Infrastructure;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XFrameOptionsMiddlewareTests {
        private const string ExampleOrgUrl = "http://www.example.org";

        public class ShouldThrowPreconditionException {
            [Fact]
            public void When_allowForm_uri_is_null() {
                Action action = () => CreateClient(b => b.AllowFrom(null));
                action.ShouldThrow<ArgumentNullException>();
            }
        }

        public class HeaderAlreadyPresent {
            [Fact]
            public async Task When_header_should_be_ignoredWhenPresent_it_should_not_modify_the_header() {
                using(var client = CreateClient(b => b.Deny().IgnoreIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XFrameOptions().Should().Be("test");
                }
            }

            [Fact]
            public async Task When_header_should_be_overwritten_it_should_be_the_expected_value() {
                using(var client = CreateClient(b => b.Deny().OverwriteHeaderIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XFrameOptions().Should().Be("DENY");
                }
            }
        }

        public class HeaderNotPresent {
            [Theory]
            [InlineData("DENY")]
            [InlineData("SAMEORIGIN")]
            [InlineData("ALLOW-FROM http://www.example.org")]
            public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
                using(var client = CreateClient(b => DetermineHeaderValue(b, expected))) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XFrameOptions().Should().Be(expected);
                }
            }

            private void DetermineHeaderValue(IFluentXfoSettingsBuilder builder, string value) {
                if(value.Equals("DENY")) {
                    builder.Deny();
                } else if(value.Equals("SAMEORIGIN")) {
                    builder.SameOrigin();
                } else {
                    builder.AllowFrom(new Uri(value.Substring(10).Trim()));
                }
            }

            [Fact]
            public async Task Default_middleware_should_add_the_header() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XFrameOptions().Should().Be("DENY");
                }
            }
        }

        private static HttpClient CreateClient(Action<IFluentXfoSettingsBuilder> settingsBuilder = null, string headerValue = null) => TestHttpClientFactory.CreateXfo(settingsBuilder, headerValue);
    }
}