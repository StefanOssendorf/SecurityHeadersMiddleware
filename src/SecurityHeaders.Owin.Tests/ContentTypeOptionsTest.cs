using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Testing;
using Owin;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Owin.Tests {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class ContentTypeOptionsTests {
        [Fact]
        public async Task When_using_contentTypeOptionsMiddleware_it_should_add_the_header() {
            var client = CtoClientHelper.Create();
            var response = await client.GetAsync("http://www.example.org");
            response.XContentTypeOptions().ShouldBe("nosniff");
        }

        [Fact]
        public async Task When_using_contentTypeOptionsMiddleware_and_header_is_already_set_it_should_net_be_overridden_with_set_configuration () {
            var client = CtoClientHelper.Create(
                ctos => ctos.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet,
                ctx => ctx.Response.Headers.Set("X-Content-Type-Options", "krznbf")
            );

            var response = await client.GetAsync("http://www.example.org");
            response.XContentTypeOptions().ShouldBe("krznbf");
        }
    }

    internal static class CtoClientHelper {

        public static HttpClient Create() {
            return Create(ctos => {});
        }

        public static HttpClient Create(Action<ContentTypeOptionsSettings> configureAction, Action<Microsoft.Owin.IOwinContext> modifyEndpoint = null) {
            return TestServer.Create(builder => {
                builder.UseOwin().ContentTypeOptions(configureAction);
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

    internal static class Extension {
        internal static BuildFunc UseOwin(this IAppBuilder builder) {
            return middleware => builder.Use(middleware(builder.Properties));
        }
    }

    internal static class HeaderHelper {
        //public static string XFrameOptionsHeader(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.XFrameOptions).First();
        //}

        //public static IEnumerable<string> StsHeader(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.StrictTransportSecurity).Single().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(value => value.Trim());
        //}

        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues("X-Content-Type-Options").Single();
        }

        //public static string XssProtection(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.XssProtection).First();
        //}

        //public static string Csp(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicy).First();
        //}

        //public static string Cspro(this HttpResponseMessage source) {
        //    return source.Headers.GetValues(HeaderConstants.ContentSecurityPolicyReportOnly).First();
        //}
    }
}