using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SecurityHeaders.Builders;
using SecurityHeaders.Tests.Infrastructure;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XXssProtectionMiddlewareTests {
        private const string ExampleOrgUrl = "http://www.example.org";

        public class ShouldThrowPreconditionException {
            [Fact]
            public void When_report_url_is_null() {
                Action action = () => CreateClient(b => b.EnabledAndReport(null));
                action.ShouldThrow<ArgumentNullException>();
            }
        }

        public class HeaderNotPresent {
            [Theory]
            [InlineData("0")]
            [InlineData("1")]
            [InlineData("1; mode=block")]
            [InlineData("1; report=http://www.example.org")]
            public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
                using(var client = CreateClient(b => DetermineHeaderValue(b, expected))) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XssProtection().Should().Be(expected);
                }
            }

            private static void DetermineHeaderValue(IFluentXpSettingsBuilder builder, string value) {
                if(value.Equals("0")) {
                    builder.Disabled();
                } else if(value.Equals("1")) {
                    builder.Enabled();
                } else if(value.StartsWith("1; mode")) {
                    builder.EnabledAndBlock();
                } else {
                    builder.EnabledAndReport(new Uri(value.Substring(10).Trim()));
                }
            }

            [Fact]
            public async Task Default_middleware_should_add_the_header() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XssProtection().Should().Be("1; mode=block");
                }
            }
        }

        public class HeaderAlreadyPresent {
            [Fact]
            public async Task When_header_should_be_ignoredIfPresent_it_should_not_modify_the_header() {
                using(var client = CreateClient(b => b.EnabledAndBlock().IgnoreIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XssProtection().Should().Be("test");
                }
            }

            [Fact]
            public async Task When_header_should_be_overwritten_it_should_be_the_expected_value() {
                using(var client = CreateClient(b => b.Enabled().OverwriteHeaderIfHeaderIsPresent(), "test")) {
                    var response = await client.GetHeaderAsync(ExampleOrgUrl);
                    response.XssProtection().Should().Be("1");
                }
            }
        }

        private static HttpClient CreateClient(Action<IFluentXpSettingsBuilder> settingsBuilder = null, string headerValue = null) => TestHttpClientFactory.CreateXp(settingsBuilder, headerValue);
    }
}