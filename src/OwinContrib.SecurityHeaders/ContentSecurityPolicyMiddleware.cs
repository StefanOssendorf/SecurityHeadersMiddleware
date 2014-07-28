using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    internal static class ContentSecurityPolicyMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentSecurityPolicyHeader(ContentSecurityPolicyConfiguration configuration) {
            return next =>
                env => {
                    var context = env.AsContext();
                    var state = new {
                        Configuration = configuration,
                        Response = context.Response
                    };

                    context.Response.OnSendingHeaders(ApplyHeader, state);

                    return next(env);
                };
        }
        private static void ApplyHeader(dynamic obj) {
            var response = (OwinResponse)obj.Response;
            var cspConfig = (ContentSecurityPolicyConfiguration)obj.Configuration;

            response.Headers[HeaderConstants.ContentSecurityPolicy] = cspConfig.ToHeaderValue();
        }
    }
}