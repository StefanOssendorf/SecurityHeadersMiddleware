using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SecurityHeaders.AspNetCore.Tests {
    public class XXssProtectionTests {
        private TestServer mServer;
        private HttpClient mClient;


        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1; mode=block")]
        [InlineData("1; report=http://www.example.org")]
        public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
            var settings = new XssProtectionSettings(DetermineHeaderValue(expected));

            Arrange(settings);
            var result = await mClient.GetAsync("/");

            result.XssProtection().Should().Be(expected);
        }

        [Fact]
        public async Task When_adding_the_default_middleware_the_header_should_be_set() {
            Arrange(null);
            var result = await mClient.GetAsync("/", HttpCompletionOption.ResponseHeadersRead);

            result.XssProtection().Should().Be("1; mode=block");
        }

        private void Arrange(XssProtectionSettings settings) {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    if (settings == null) {
                        app.UseXssProtection();
                    } else {
                        app.UseXssProtection(() => settings);
                    }
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            mServer = new TestServer(builder);
            mClient = mServer.CreateClient();
        }

        private static XssProtectionHeaderValue DetermineHeaderValue(string value) {
            if(value.Equals("0")) {
                return XssProtectionHeaderValue.Disabled();
            }
            if(value.Equals("1")) {
                return XssProtectionHeaderValue.Enabled();
            }
            if (value.StartsWith("1; mode")) {
                return XssProtectionHeaderValue.EnabledAndBlock();
            }

            return XssProtectionHeaderValue.EnabledAndReport(value.Substring(10).Trim());
        }
    }
}