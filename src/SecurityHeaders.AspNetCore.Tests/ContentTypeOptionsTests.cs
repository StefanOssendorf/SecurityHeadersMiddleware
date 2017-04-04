using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace SecurityHeaders.AspNetCore.Tests {
    public class ContentTypeOptionsTests {
        private TestServer mServer;
        private HttpClient mClient;
        private Action<HttpContext> mModifyRun;
        private bool mUseDefault;
        private Func<ContentTypeOptionsSettings> mSetSettings = () => new ContentTypeOptionsSettings();


        private void Arrange() {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    if(mUseDefault) {
                        app.UseContentTypeOptions();
                    } else {
                        app.UseContentTypeOptions(mSetSettings);
                    }
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        mModifyRun?.Invoke(ctx);
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            mServer = new TestServer(builder);
            mClient = mServer.CreateClient();
        }

        [Fact]
        public async Task When_adding_with_default_settings_it_should_add_the_header() {
            mUseDefault = true;
            Arrange();
            var result = await mClient.GetAsync("/");
            result.XContentTypeOptions().Should().Be("nosniff");
        }

        [Fact]
        public async Task When_adding_header_with_do_not_override_and_header_already_exist_it_should_not_be_overridden() {
            mModifyRun = ctx => ctx.Response.Headers.Add(ContentTypeOptionsMiddleware.XContentTypeOptionsHeaderName, "invalidvalue");
            mSetSettings = () => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
            Arrange();
            var result = await mClient.GetAsync("/");
            result.XContentTypeOptions().Should().Be("invalidvalue");
        }
    }
}
