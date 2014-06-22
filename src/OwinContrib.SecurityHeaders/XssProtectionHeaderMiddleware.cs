using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders {
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;

    internal static class XssProtectionHeaderMiddleware {
        public static MidFunc XssProtectionHeader() {
            return XssProtectionHeader(false);
        }
        public static MidFunc XssProtectionHeader(bool disabled) {
            return
                next =>
                    env => {
                        var response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => {
                                var value = disabled ? "0" : "1; mode=block";
                                ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = value;
                            }, response);
                        return next(env);
                    };
        }
    }
}