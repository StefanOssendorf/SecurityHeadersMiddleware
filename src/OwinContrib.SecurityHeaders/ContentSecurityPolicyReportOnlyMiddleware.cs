using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class ContentSecurityPolicyReportOnlyMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentSecurityPolicyHeader(ContentSecurityPolicyConfiguration configuration) {
            return next =>
                env => {
                    var context = env.AsContext();
                    var state = new State {
                        Configuration = configuration,
                        Response = context.Response
                    };
                    context.Response.OnSendingHeaders(ApplyHeader, state);
                    return next(env);
                };
        }

        private static void ApplyHeader(State obj) {
            var response = obj.Response;
            var cspConfig = obj.Configuration;

            if (ContainsCspHeader(response.Headers)) {
                // "A server MUST NOT send more than one HTTP header field named Content-Security-Policy-Report-Only with a given resource representation."
                // Source: http://www.w3.org/TR/CSP2/#content-security-policy-report-only-header-field  (Date: 06.04.2015)
                return;
            }

            response.Headers[HeaderConstants.ContentSecurityPolicyReportOnly] = cspConfig.ToHeaderValue();
        }

        private static bool ContainsCspHeader(IHeaderDictionary headers) {
            return headers.ContainsKey(HeaderConstants.ContentSecurityPolicy);
        }

        private class State {
            public ContentSecurityPolicyConfiguration Configuration { get; set; }
            public IOwinResponse Response { get; set; }
        }
    }
}