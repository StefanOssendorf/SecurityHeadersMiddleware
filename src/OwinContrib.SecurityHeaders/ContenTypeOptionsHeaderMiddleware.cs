using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders {
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;

    internal static class ContenTypeOptionsHeaderMiddleware {
        public static MidFunc ContentTypeOptionsHeader() {
            return next =>
                env => {
                    var response = env.AsContext().Response;
                    response.OnSendingHeaders(ApplyHeader, response);
                    return next(env);
                };
        }
        private static void ApplyHeader(object obj) {
            var response = (IOwinResponse)obj;
            response.Headers[HeaderConstants.XContentTypeOptions] = "nosniff";
        }
    }
}