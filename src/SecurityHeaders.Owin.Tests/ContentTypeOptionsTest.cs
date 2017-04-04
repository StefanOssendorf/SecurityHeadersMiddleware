using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Owin;
using Xunit;

namespace SecurityHeaders.Owin.Tests {
    public class ContentTypeOptionsTests {
        [Fact]
        public async Task When_using_contentTypeOptionsMiddleware_it_should_add_the_header() {
            var client = CtoClientHelper.Create();
            var response = await client.GetAsync("http://www.example.org");
            response.XContentTypeOptions().Should().Be("nosniff");
        }

        [Fact]
        public async Task When_using_contentTypeOptionsMiddleware_and_header_is_already_set_it_should_net_be_overridden_with_set_configuration() {
            var client = CtoClientHelper.Create(
                () => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet),
                ctx => ctx.Response.Headers.Set("X-Content-Type-Options", "krznbf")
            );

            var response = await client.GetAsync("http://www.example.org");
            response.XContentTypeOptions().Should().Be("krznbf");
        }


        private static class CtoClientHelper {

            public static HttpClient Create() => Create(null);

            public static HttpClient Create(Func<ContentTypeOptionsSettings> configureAction, Action<Microsoft.Owin.IOwinContext> modifyEndpoint = null) {
                return TestServer.Create(builder => {
                    if (configureAction == null) {
                        builder.UseOwin().ContentTypeOptions();
                    } else {
                        builder.UseOwin().ContentTypeOptions(configureAction);
                    }
                    
                    builder
                        .Use((context, next) => {
                            context.Response.StatusCode = 200;
                            context.Response.ReasonPhrase = "OK";
                            modifyEndpoint?.Invoke(context);
                            return Task.FromResult(0);
                        });
                }).HttpClient;
            }
        }
    }
}