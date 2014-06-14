using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;
using OwinContrib.Security.Infrastructure;

namespace OwinContrib.Security.Tests {
    public abstract class AntiClickjackingSpecBase {
        protected static HttpClient Subject;
        protected static HttpResponseMessage Response;

        private Because of = () => { Response = Subject.GetAsync("http://example.com").Await(); };
    }

    [Subject(typeof(AntiClickjackingMiddleware))]
    public class When_using_use_with_no_parameter : AntiClickjackingSpecBase {
        private Establish context = () => Subject = ClientHelper.CreateClient();

        private It should_set_xFrameOptions_header_to_deny = () => Response.XFrameOptionsHeader().ShouldEqual("DENY");
    }

    [Subject(typeof(AntiClickjackingMiddleware))]
    public class When_using_use_with_xFrameOption_deny : AntiClickjackingSpecBase {
        private Establish context = () => Subject = ClientHelper.CreateClient(XFrameOption.Deny);
        private It should_set_xFrameOptions_header_to_deny = () => Response.XFrameOptionsHeader().ShouldEqual("DENY");
    }

    [Subject(typeof(AntiClickjackingMiddleware))]
    public class When_using_use_with_xFrameOption_sameOrigin : AntiClickjackingSpecBase {
        private Establish context = () => Subject = ClientHelper.CreateClient(XFrameOption.SameOrigin);
        private It should_set_xFrameOptions_header_to_sameOrigin = () => Response.XFrameOptionsHeader().ShouldEqual("SAMEORIGIN");
    }

    [Subject(typeof(AntiClickjackingMiddleware))]
    public class When_using_use_with_example_com_origin : AntiClickjackingSpecBase {
        private Establish context = () => Subject = ClientHelper.CreateClient("http://example.com");
        private It should_set_xFrameOptions_header_to_allowFrom_example_com_origin = () => Response.XFrameOptionsHeader().ShouldEqual("ALLOW-FROM http://example.com");
    }

    [Subject(typeof(AntiClickjackingMiddleware))]
    public class When_using_use_with_multiple_origins_configured {
        private static HttpClient Subject;

        private Establish context = () => Subject = ClientHelper.CreateClient("http://example.com", "http://example.org");
        private It should_set_xFrameOptions_header_to_allowFrom_example_com_origin = () => {
            HttpResponseMessage response = Subject.GetAsync("http://example.com").Await();
            response.XFrameOptionsHeader().ShouldEqual("ALLOW-FROM http://example.com");
        };
        private It should_set_xFrameOptions_header_to_allowFrom_example_org_origin = () => {
            HttpResponseMessage response = Subject.GetAsync("http://example.org").Await();
            response.XFrameOptionsHeader().ShouldEqual("ALLOW-FROM http://example.org");
        };
        private It should_set_xFrameOptions_header_to_deny_with_unknown_origin = () => {
            HttpResponseMessage response = Subject.GetAsync("http://unknown.org").Await();
            response.XFrameOptionsHeader().ShouldEqual("DENY");
        };
    }

    internal static class ClientHelper {
        public static HttpClient CreateClient() {
            return CreateClient(b => b.Use().AntiClickjackingHeader());
        }
        public static HttpClient CreateClient(XFrameOption option) {
            return CreateClient(b => b.Use().AntiClickjackingHeader(option));
        }
        public static HttpClient CreateClient(params string[] origins) {
            return CreateClient(b => b.Use().AntiClickjackingHeader(origins));
        }
        public static HttpClient CreateClient(params Uri[] origins) {
            return CreateClient(b => b.Use().AntiClickjackingHeader(origins));
        }
        private static HttpClient CreateClient(Action<IAppBuilder> registerAntiClickjackingMiddleware) {
            return TestServer.Create(builder => {
                registerAntiClickjackingMiddleware(builder);
                builder
                    .Use((context, next) => {
                        context.Response.StatusCode = 200;
                        context.Response.ReasonPhrase = "OK";
                        return Task.FromResult(0);
                    });
            }).HttpClient;
        }
    }

    internal static class HeaderHelper {
        public static string XFrameOptionsHeader(this HttpResponseMessage source) {
            return source.Headers.GetValues(HeaderConstants.XFrameOptions).First();
        }
    }
}