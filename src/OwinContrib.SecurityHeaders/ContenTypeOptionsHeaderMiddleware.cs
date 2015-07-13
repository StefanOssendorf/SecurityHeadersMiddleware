using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class ContenTypeOptionsHeaderMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentTypeOptionsHeader() {
            return next =>
                env => {
                    var response = env.AsContext().Response;
                    response.OnSendingHeaders(ApplyHeader, response);
                    return next(env);
                };
        }

        private static void ApplyHeader(IOwinResponse response) {
            //TODO Introduce new setting option to specify the behavior
            response.Headers[HeaderConstants.XContentTypeOptions] = "nosniff";
        }
    }
}