using System.Linq;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders.Tests {
    [Subject(typeof(XssProtectionHeaderMiddleware))]
    public class XssProtectionHeaderMiddlewareSpecs : OwinEnvironmentSpecBase {
        private Establish context = () => Client = TestServer.Create(app => {
            app.Use().XssProtectionHeader();
            app.Run(async context => await context.Response.WriteAsync("All fine"));
        }).HttpClient;

        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();

        private It Should_have_header_in_response = () => Response.Headers.GetValues(HeaderConstants.XssProtection).First().ShouldEqual("1; mode=block");
    }
}