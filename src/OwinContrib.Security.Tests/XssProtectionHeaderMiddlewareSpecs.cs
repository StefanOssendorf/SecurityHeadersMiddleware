using System.Linq;
using System.Net.Http;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;

namespace OwinContrib.Security.Tests {
    [Subject(typeof(XssProtectionHeaderMiddleware))]
    public class XssProtectionHeaderMiddlewareSpecs {
        private static TestServer Server;
        private static HttpResponseMessage Response;
        private It Should_have_header_in_response = () => Response.Headers.GetValues(HeaderConstants.XssProtection).First().ShouldEqual("1; mode=block");

        private Establish context = () => Server = TestServer.Create(app => {
            app.Use().XssProtectionHeader();
            app.Run(async context => await context.Response.WriteAsync("All fine"));
        });

        private Because of = () => Response = Server.HttpClient.GetAsync("http://www.example.org").Await();
    }
}