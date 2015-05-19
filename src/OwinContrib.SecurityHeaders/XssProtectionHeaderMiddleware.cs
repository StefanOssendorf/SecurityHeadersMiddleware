using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class XssProtectionHeaderMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader() {
            return XssProtectionHeader(false);
        }

        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader(bool disabled) {
            return
                next =>
                    env => {
                        var response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => {
                                var value = disabled ? "0" : "1; mode=block";
                                ((IOwinResponse) resp).Headers[HeaderConstants.XssProtection] = value;
                            }, response);
                        return next(env);
                    };
        }
    }
}