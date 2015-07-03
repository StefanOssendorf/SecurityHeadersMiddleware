using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Owin.Testing;
using Owin;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.OwinAppBuilder;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    [Subject(typeof (StrictTransportSecurityHeaderMiddleware))]
    public class When_using_sts_default_implementation_over_non_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => { Client = StsClientHelper.Create(); };
        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();
        private It Should_not_return_sts_header = () => Response.Headers.Contains(HeaderConstants.StrictTransportSecurity).ShouldBeFalse();
        private It should_not_send_a_location_header = () => Response.Headers.Location.ShouldBeNull();
        private It Should_not_set_statusCode_to_301 = () => Response.StatusCode.ShouldNotEqual(HttpStatusCode.MovedPermanently);
    }

    [Subject(typeof (StrictTransportSecurityHeaderMiddleware))]
    public class When_using_sts_default_implementation_over_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => { Client = StsClientHelper.Create(); };
        private Because of = () => Response = Client.GetAsync("https://www.example.org").Await();
        private It should_contain_sts_header = () => Response.StsHeader().Any().ShouldBeTrue();
        private It Should_have_includeSubDomains_as_value = () => Response.StsHeader().ShouldContain("includeSubDomains");
        private It should_have_maxAge_with_31536000 = () => Response.StsHeader().Single(h => h.StartsWith("max-age=", StringComparison.OrdinalIgnoreCase)).ShouldBeEqualIgnoringCase("max-age=31536000");
    }

    [Subject(typeof (StrictTransportSecurityHeaderMiddleware))]
    public class When_using_configured_sts_over_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => {
            Client = StsClientHelper.Create(
                new StrictTransportSecurityOptions {
                    IncludeSubDomains = false,
                    MaxAge = 0
                });
        };

        private Because of = () => Response = Client.GetAsync("https://www.example.org").Await();
        private It should_have_maxAge_with_0 = () => Response.StsHeader().ShouldContain(hv => hv.EqualsIgnoreCase("max-age=0"));
        private It should_not_contain_includeSubDomains = () => Response.StsHeader().ShouldNotContain(hv => hv.EqualsIgnoreCase("includeSubDomains"));
    }

    [Subject(typeof (StrictTransportSecurityHeaderMiddleware))]
    public class When_using_configured_sts_over_non_secure_transport : OwinEnvironmentSpecBase {
        private Establish context = () => {
            Client = StsClientHelper.Create(
                new StrictTransportSecurityOptions {
                    RedirectToSecureTransport = true,
                    RedirectReasonPhrase = sc => "Custom Resonphrase"
                });
        };

        private Because of = () => Response = Client.GetAsync("http://www.example.org").Await();
        private It should_set_custom_reasonPhrase = () => Response.ReasonPhrase.ShouldEqual("Custom Resonphrase");
        private It should_set_location_hedaer_to_https_scheme_uri = () => Response.Headers.Location.ShouldEqual(new Uri("https://www.example.org"));
        private It should_set_statusCode_to_301 = () => Response.StatusCode.ShouldEqual(HttpStatusCode.MovedPermanently);
    }

    public class StrictTransportSecurityTests {
        [Fact]
        public async Task When_redirecting_to_https_the_default_should_return_no_reasonPhrase() {
            var client = StsClientHelper.Create(new StrictTransportSecurityOptions {
                RedirectToSecureTransport = true
            });
            var response = await client.GetAsync("http://www.example.org");
            response.ReasonPhrase.Should().BeEmpty();
        }

        [Fact]
        public async Task When_preload_is_set_it_should_be_included_in_the_response_header_value() {
            var client = StsClientHelper.Create(new StrictTransportSecurityOptions {
                Preload = true,
                MaxAge = 50
            });
            var response = await client.GetAsync("https://wwww.example.org");
            response.StsHeader().Should().Contain("preload");
        }
    }

    internal class StsClientHelper {
        public static HttpClient Create() {
            return Create(new StrictTransportSecurityOptions());
        }

        public static HttpClient Create(StrictTransportSecurityOptions strictTransportSecurityOptions) {
            return TestServer.Create(builder => {
                builder.UseOwin().StrictTransportSecurity(strictTransportSecurityOptions);
                builder.Use((context, next) => {
                    context.Response.StatusCode = 200;
                    context.Response.ReasonPhrase = "OK";
                    return Task.FromResult(0);
                });
            }).HttpClient;
        }
    }

    internal static class StringExtensions {
        public static bool EqualsIgnoreCase(this string source, string toCompare) {
            return source.Equals(toCompare, StringComparison.OrdinalIgnoreCase);
        }
    }
}