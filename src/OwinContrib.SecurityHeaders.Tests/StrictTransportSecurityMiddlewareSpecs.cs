using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders.Tests {
    [Subject(typeof(StrictTransportSecurityMiddleware))]
    public class When_using_sts_default_implementation_over_non_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => { Client = StsClientHelper.Create(); };

        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();
        private It Should_not_return_sts_header = () => Response.Headers.Contains(HeaderConstants.StrictTransportSecurity).ShouldBeFalse();
        private It Should_not_set_statusCode_to_301 = () => Response.StatusCode.ShouldNotEqual(HttpStatusCode.MovedPermanently);
    }

    [Subject(typeof(StrictTransportSecurityMiddleware))]
    public class When_using_sts_default_implementation_over_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => { Client = StsClientHelper.Create(); };

        private Because of = () => Response = Client.GetAsync("https://www.example.org").Await();

        private It should_contain_sts_header = () => Response.StsHeader().Any().ShouldBeTrue();
        private It should_have_maxAge_with_31536000 = () => Response.StsHeader().Single(h => h.StartsWith("max-age=", StringComparison.OrdinalIgnoreCase)).ShouldBeEqualIgnoringCase("max-age=31536000");
        private It Should_have_includeSubDomains_as_value = () => Response.StsHeader().ShouldContain("includeSubDomains");
        
    }

    internal class StsClientHelper {
        public static HttpClient Create() {
            return TestServer.Create(builder => {
                builder.Use().StrictTransportSecurity();
                builder.Use((context, next) => {
                    context.Response.StatusCode = 200;
                    context.Response.ReasonPhrase = "OK";
                    return Task.FromResult(0);
                });
            }).HttpClient;
        }
    }
}