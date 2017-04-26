using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SecurityHeaders.Builders;
using Xunit;

namespace SecurityHeaders.Tests {
    public class StrictTransportSecurityMiddlewareTests {
        private const string HttpExampleOrgUrl = "http://www.example.org";
        private const string HttpsExampleOrgUrl = "http://www.example.org";

        public class HeaderAlreadyPresent {
            
        }

        public class HeaderNotPresent {
            [Fact]
            public async Task Default_middleware_should_add_the_header_on_https() {
                using(var client = CreateClient()) {
                    var response = await client.GetHeaderAsync(HttpsExampleOrgUrl);
                    var headerValues = response.StrictTransportSecurity();
                    headerValues.Count.Should().Be(2);
                    headerValues[0].Should().Be("max-age=31536000");
                    headerValues[1].Should().Be("includeSubDomains");
                }
            }

            
        }

        private static HttpClient CreateClient(Action<IFluentStsMaxAgeSettingsBuilder> settingsBuilder = null, string headerValue = null) => TestHttpClientFactory.CreateSts(settingsBuilder, headerValue);

        //public class BeforeNext {
        //    [Fact]
        //    public void When_connection_is_not_secure_and_it_should_not_be_redirected_the_pipeline_should_not_be_ended() {
        //        var mw = CreateSts(false);
        //        var ctx = new TestContext() {
        //            IsSecure = false,
        //            RequestUri = new Uri("http://www.exmpale.org")
        //        };

        //        var result = mw.BeforeNext(ctx);
        //        result.EndPipeline.Should().BeFalse();
        //    }


        //    [Fact]
        //    public void When_connection_is_not_secure_and_it_should_be_redirected_the_pipeline_should_be_ended() {
        //        var mw = CreateSts();
        //        var ctx = new TestContext {
        //            IsSecure = false,
        //            RequestUri = new Uri("http://www.exmpale.org"),
        //            PermanentRedirectToAction = a => { }
        //        };

        //        var result = mw.BeforeNext(ctx);
        //        result.EndPipeline.Should().BeTrue();
        //    }

        //    [Fact]
        //    public void When_connection_is_not_secure_and_it_should_be_redirected_the_permanentRedirect_method_should_be_called_with_expected_values() {
        //        var mw = CreateSts();
        //        bool redirectCalled = false;
        //        var ctx = new TestContext() {
        //            IsSecure = false,
        //            PermanentRedirectToAction = a => redirectCalled = true,
        //            RequestUri = new Uri("http://www.exmpale.org")
        //        };

        //        mw.BeforeNext(ctx);

        //        redirectCalled.Should().BeTrue();
        //    }
        //}


        //[Fact]
        //public void When_connection_is_not_secure_and_it_should_not_be_redirected_the_header_should_not_be_set() {
        //    var middleware = CreateSts();

        //    var ctx = new TestContext() {
        //        HeaderExistFunc = _ => true,
        //        IsSecure = false
        //    };

        //    Action action = () => middleware.ApplyHeader(ctx);
        //    action.ShouldNotThrow();
        //}


        //private static StrictTransportSecurityMiddleware CreateSts() {
        //    return CreateSts(true);
        //}
        //private static StrictTransportSecurityMiddleware CreateSts(bool permanentRedirect) {
        //    return new StrictTransportSecurityMiddleware(new StrictTransportSecuritySettings(permanentRedirect));
        //}
    }
}