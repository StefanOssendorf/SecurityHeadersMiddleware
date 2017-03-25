using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
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

            result.XFrameOptions().ShouldBe(expected);
        }

        private void Arrange(AntiClickjackingSettings settings) {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    app.UseAntiClickjacking(() => settings);
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            mServer = new TestServer(builder);
            mClient = mServer.CreateClient();
        }

        private static XFrameOptionHeaderValue DetermineHeaderValue(string value) {
            if(value.Equals("DENY")) {
                return XFrameOptionHeaderValue.Deny();
            }
            if(value.Equals("SAMEORIGIN")) {
                return XFrameOptionHeaderValue.SameOrigin();
            }

            return XFrameOptionHeaderValue.AllowFrom(value.Substring(10).Trim());
        }
    }
}