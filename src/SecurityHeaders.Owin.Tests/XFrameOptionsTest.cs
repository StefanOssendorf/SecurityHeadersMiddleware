using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Owin;
using Xunit;

namespace SecurityHeaders.Owin.Tests {
    public class XFrameOptionsTest {
        
        [Theory]
        [InlineData("DENY")]
        [InlineData("SAMEORIGIN")]
        [InlineData("ALLOW-FROM http://www.example.org")]
        public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
            var settings = new AntiClickjackingSettings(DetermineHeaderValue(expected));
            var client = XfoClientHelper.Create(() => settings);

            var response = await client.GetAsync("http://www.example.org");

            response.XFrameOptions().Should().Be(expected);
        }

        [Fact]
        public async Task When_adding_default_middleware_header_should_be_set() {
            var client = XfoClientHelper.Create();

            var response = await client.GetAsync("http://www.exmpale.org");

            response.XFrameOptions().Should().Be("DENY");
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

        private static class XfoClientHelper {
            public static HttpClient Create() => Create(null);

            public static HttpClient Create(Func<AntiClickjackingSettings> configureAction,
                Action<Microsoft.Owin.IOwinContext> modifyEndpoint = null) {
                return TestServer.Create(builder => {
                    if (configureAction == null) {
                        builder.UseOwin().AntiClickjacking();
                    } else {
                        builder.UseOwin().AntiClickjacking(configureAction);
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