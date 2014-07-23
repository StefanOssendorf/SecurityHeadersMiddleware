using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityHeadersMiddleware {
    internal static class ContentSecurityPolicyMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentSecurityPolicyHeader() {
            return next =>
                env => {
                    return next(env);
                };
        }
    }
}