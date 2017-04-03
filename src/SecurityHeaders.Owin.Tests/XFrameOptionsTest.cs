using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Owin;
using Shouldly;
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

            response.XFrameOptions().ShouldBe(expected);
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

            public static HttpClient Create(Func<AntiClickjackingSettings> configureAction,
                Action<Microsoft.Owin.IOwinContext> modifyEndpoint = null) {
                return TestServer.Create(builder => {
                    builder.UseOwin().AntiClickjacking(configureAction);
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


    public class XXssProtectionTests {
        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1; mode=block")]
        [InlineData("1; report=http://www.example.org")]
        public async Task When_adding_the_header_it_should_have_the_appropiate_value(string expected) {
            var settings = new XssProtectionSettings(DetermineHeaderValue(expected));
            var client = XpoClientHelper.Create(() => settings);

            var response = await client.GetAsync("http://www.example.org");

            response.XssProtection().ShouldBe(expected);
        }

        private static XssProtectionHeaderValue DetermineHeaderValue(string value) {
            if(value.Equals("0")) {
                return XssProtectionHeaderValue.Disabled();
            }
            if(value.Equals("1")) {
                return XssProtectionHeaderValue.Enabled();
            }
            if(value.StartsWith("1; mode")) {
                return XssProtectionHeaderValue.EnabledAndBlock();
            }

            return XssProtectionHeaderValue.EnabledAndReport(value.Substring(10).Trim());
        }

        private static class XpoClientHelper {

            public static HttpClient Create(Func<XssProtectionSettings> configureAction,
                Action<Microsoft.Owin.IOwinContext> modifyEndpoint = null) {
                return TestServer.Create(builder => {
                    builder.UseOwin().XssProtection(configureAction);
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