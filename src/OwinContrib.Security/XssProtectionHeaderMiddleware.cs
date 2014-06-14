using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using OwinContrib.Security.Infrastructure;

namespace OwinContrib.Security {
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;    
    public static class XssProtectionHeaderMiddleware {
        /// <summary>
        /// Adds the "X-Xss-Protection" header with value "1; mode=block".
        /// </summary>
        /// <returns></returns>
        public static MidFunc XssProtectionHeader() {
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