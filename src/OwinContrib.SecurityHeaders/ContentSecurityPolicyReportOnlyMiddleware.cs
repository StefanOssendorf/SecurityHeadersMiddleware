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
                    IOwinContext context = env.AsContext();
                    var state = new {
                        Configuration = configuration,
                        context.Response
                    };
                    context.Response.OnSendingHeaders(ApplyHeader, state);
                    return next(env);
                };
        }
        private static void ApplyHeader(dynamic obj) {
            var response = (OwinResponse)obj.Response;
            var cspConfig = (ContentSecurityPolicyConfiguration)obj.Configuration;
            response.Headers[HeaderConstants.ContentSecurityPolicyReportOnly] = cspConfig.ToHeaderValue();
        }
    }
}