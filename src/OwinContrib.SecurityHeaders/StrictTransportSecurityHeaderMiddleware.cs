using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class StrictTransportSecurityHeaderMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> StrictTransportSecurityHeader(StrictTransportSecurityOptions options) {
            return next =>
                env => {
                    var context = env.AsContext();
                    var request = context.Request;
                    if (RedirectToSecureTransport(options, request)) {
                        SetResponseForRedirect(context, options);
                        return Task.FromResult(0);
                    }

                    // Only over secure transport (http://tools.ietf.org/html/rfc6797#section-7.2)
                    // Quotation: "An HSTS Host MUST NOT include the STS header field in HTTP responses conveyed over non-secure transport."
                    if (request.IsSecure) {
                        var response = context.Response;
                        var state = new {
                            Options = options,
                            Response = response
                        };
                        response.OnSendingHeaders(ApplyHeader, state);
                    }
                    return next(env);
                };
        }

        private static void SetResponseForRedirect(IOwinContext context, StrictTransportSecurityOptions options) {
            var response = context.Response;
            response.StatusCode = 301;
            response.ReasonPhrase = options.RedirectReasonPhrase(301);
            response.Headers[HeaderConstants.Location] = options.RedirectUriBuilder(context.Request.Uri);
        }

        private static void ApplyHeader(dynamic obj) {
            var options = (StrictTransportSecurityOptions) obj.Options;
            var response = (OwinResponse) obj.Response;
            response.Headers[HeaderConstants.StrictTransportSecurity] = ConstructHeaderValue(options);
        }

        private static string ConstructHeaderValue(StrictTransportSecurityOptions options) {
            var age = MaxAge(options.MaxAge);
            var subDomains = IncludeSubDomains(options.IncludeSubDomains);
            return "{0}{1}".FormatWith(age, subDomains);
        }

        #region [ Helper ]

        private static string MaxAge(uint seconds) {
            return "max-age={0}".FormatWith(seconds);
        }

        private static string IncludeSubDomains(bool includeSubDomains) {
            return includeSubDomains ? "; includeSubDomains" : "";
        }

        private static bool RedirectToSecureTransport(StrictTransportSecurityOptions options, IOwinRequest request) {
            return options.RedirectToSecureTransport && !request.IsSecure;
        }

        #endregion
    }
}