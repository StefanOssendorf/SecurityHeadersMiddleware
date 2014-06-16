using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders {
    public static class XssProtectionHeaderMiddleware {
        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block".
        /// </summary>
        /// <returns></returns>
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader() {
            return
                next =>
                    env => {
                        var response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => { ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = "1; mode=block"; }, response);
                        return next(env);
                    };
        }
    }
}