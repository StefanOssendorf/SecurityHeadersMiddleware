using System.Net.Http;
using System.Threading.Tasks;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;

namespace OwinContrib.SecurityHeaders.Tests {
    [Subject(typeof(ContenTypeOptionsHeaderMiddleware))]
    public class When_using_contentTypeOptionsHeaderMiddleware : OwinEnvironmentSpecBase {
        private Establish context = () => Client = CtoClientHelper.Create();

        private Because of = () => Response = Client.GetAsync("http://wwww.example.org").Await();

        private It should_contain_contentTypeOptions_header = () => Response.XContentTypeOptions().ShouldNotBeNull();
        private It should_containt_nosniff_as_header_value = () => Response.XContentTypeOptions().ShouldEqual("nosniff");
    }

    internal static class CtoClientHelper {
        public static HttpClient Create() {
            return TestServer.Create(builder => {
                builder.UseOwin().ContentTypeOptions();
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