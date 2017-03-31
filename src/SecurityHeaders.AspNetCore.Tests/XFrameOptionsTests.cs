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

    public class AspNetCoreHttpContextTest {
        [Fact]
        public void When_append_to_a_header_and_header_is_not_present_it_should_be_added() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.AppendToHeader("Test", "Test");

            httpContext.Response.Headers.Keys.ShouldContain("Test");
            httpContext.Response.Headers["Test"].Equals("Test").ShouldBeTrue();
        }

        [Fact]
        public void When_appending_to_a_header_it_should_be_appended() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Abcd" });

            internalContext.AppendToHeader("Test", "Test");

            httpContext.Response.Headers["Test"].ToString().ShouldBe("Abcd,Test");
        }

        [Fact]
        public void When_checking_if_an_absent_header_exist_it_should_return_false() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.HeaderExist("Test").ShouldBeFalse();
        }

        [Fact]
        public void When_checking_if_a_present_header_exist_it_should_return_true() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Test" });

            internalContext.HeaderExist("Test").ShouldBeTrue();
        }

        [Fact]
        public void When_overriding_an_absent_header_it_should_be_added() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.OverrideHeader("Test", "Test");

            httpContext.Response.Headers.Keys.ShouldContain("Test");
            httpContext.Response.Headers["Test"].Equals("Test").ShouldBeTrue();
        }

        [Fact]
        public void When_overriding_a_header_the_value_should_be_overridden() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Test" });

            internalContext.OverrideHeader("Test", "krznbf");

            httpContext.Response.Headers["Test"].Equals("krznbf").ShouldBeTrue();
        }
    }
}