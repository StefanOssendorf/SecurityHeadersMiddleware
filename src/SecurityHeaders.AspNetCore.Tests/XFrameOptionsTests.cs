using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SecurityHeaders.AspNetCore.Tests {
    public class XFrameOptionsTests {
        private TestServer mServer;
        private HttpClient mClient;
       

        [Theory]
        [InlineData("DENY")]
        [InlineData("SAMEORIGIN")]
        [InlineData("ALLOW-FROM http://www.example.org")]
        public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
            var settings = new AntiClickjackingSettings(DetermineHeaderValue(expected));

            Arrange(settings);
            var result = await mClient.GetAsync("/");

            result.XFrameOptions().Should().Be(expected);
        }

        [Fact]
        public async Task When_adding_the_default_middleware_the_header_should_be_set() {
            Arrange(null);
            var result = await mClient.GetAsync("/", HttpCompletionOption.ResponseHeadersRead);

            result.XFrameOptions().Should().Be("DENY");
        }

        private void Arrange(AntiClickjackingSettings settings) {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    if (settings == null) {
                        app.UseAntiClickjacking();
                    } else {
                        app.UseAntiClickjacking(() => settings);
                    }
                    
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            mServer = new TestServer(builder);
            mClient = mServer.CreateClient();
        }

        private static AntiClickjackingHeaderValue DetermineHeaderValue(string value) {
            if(value.Equals("DENY")) {
                return AntiClickjackingHeaderValue.Deny();
            }
            if(value.Equals("SAMEORIGIN")) {
                return AntiClickjackingHeaderValue.SameOrigin();
            }

            return AntiClickjackingHeaderValue.AllowFrom(value.Substring(10).Trim());
        }
    }
}