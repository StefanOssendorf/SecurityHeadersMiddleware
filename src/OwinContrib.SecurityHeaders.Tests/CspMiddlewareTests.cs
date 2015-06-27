using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Owin;
using Microsoft.Owin.Testing;
using Owin;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.OwinAppBuilder;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspMiddlewareTests {
        [Fact]
        public async Task When_adding_csp_middleware_a_response_should_serve_the_csp_header() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https:");
            config.ImgSrc.AddKeyword(SourceListKeyword.Self);
            var client = CspClientHelper.Create(config);
            var response = await client.GetAsync("https://wwww.example.com");
            response.Csp().Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task When_adding_csp_middleware_the_response_should_contain_the_expected_csp_directives() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https:");
            config.ImgSrc.AddKeyword(SourceListKeyword.Self);
            var client = CspClientHelper.Create(config);
            var response = await client.GetAsync("https://wwww.example.com");
            var headerValue = response.Csp();
            var values = headerValue.Split(new[] {";"}, StringSplitOptions.None).Select(i => i.Trim()).ToList();
            values.Count.Should().Be(2);
            values.Should().Contain(i => i.Equals("img-src 'self'"));
            values.Should().Contain(i => i.Equals("script-src https:"));
        }

        [Fact]
        public async Task When_adding_csp_middleware_and_another_middleware_has_already_added_a_csp_header_the_middlewar_should_not_add_the_header() {
            var cfg = new ContentSecurityPolicyConfiguration();
            cfg.ScriptSrc.AddKeyword(SourceListKeyword.Self);
            var client = CspClientHelper.Create(cfg,
                builder => builder.Use(async (ctx,next) => {
                    ctx.Response.OnSendingHeaders(ctx2 => {
                        ((IOwinResponse)ctx2).Headers.Add(HeaderConstants.ContentSecurityPolicy, new []{"Dummy"});
                    }, ctx.Response);
                    await next();
                }));
            var resp = await client.GetAsync("http://www.example.com");
            var header = resp.Csp();
            header.ShouldEqual("Dummy");
        }
    }

    internal static class CspClientHelper {
        public static HttpClient Create(ContentSecurityPolicyConfiguration configuration, Action<IAppBuilder> intercepter = null) {
            return TestServer.Create(builder => {
                intercepter?.Invoke(builder);
                builder.UseOwin().ContentSecurityPolicy(configuration);
                builder
                    .Use((context, next) => {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    });
            }).HttpClient;
        }

    }
}