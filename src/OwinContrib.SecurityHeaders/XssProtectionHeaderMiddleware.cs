using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    internal static class XssProtectionHeaderMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader() {
            return XssProtectionHeader(false);
        }
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader(bool disabled) {
            return
                next =>
                    env => {
                        IOwinResponse response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => {
                                string value = disabled ? "0" : "1; mode=block";
                                ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = value;
                            }, response);
                        return next(env);
                    };
        }
    }
}