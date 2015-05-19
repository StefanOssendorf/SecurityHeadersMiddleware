using System.Net.Http;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Tests {
    [Subject(typeof (XssProtectionHeaderMiddleware))]
    public class When_protection_is_enabled : OwinEnvironmentSpecBase {
        private Establish context = () => Client = XssClientHelper.CreateClient(false);
        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();
        private It Should_have_header_in_response = () => Response.XssProtection().ShouldEqual("1; mode=block");
    }

    [Subject(typeof (XssProtectionHeaderMiddleware))]
    public class When_disabling_protection : OwinEnvironmentSpecBase {
        private Establish context = () => Client = XssClientHelper.CreateClient(true);
        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();
        private It should_have_header_with_value_0 = () => Response.XssProtection().ShouldEqual("0");
    }

    internal static class XssClientHelper {
        public static HttpClient CreateClient(bool disabled) {
            return TestServer.Create(app => {
                app.UseOwin().XssProtectionHeader(disabled);
                app.Run(async context => await context.Response.WriteAsync("All fine"));
            }).HttpClient;
        }
    }
}