using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class XssProtectionHeaderMiddleware {
        //TODO Introduce new setting option to specify the behavior
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader(XssProtectionOption option) {
            return
                next =>
                    env => {
                        var response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => {
                                
                                ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = GetHeaderValue(option);
                            }, response);
                        return next(env);
                    };
        }

        private static string GetHeaderValue(XssProtectionOption option) {
            switch (option) {
                case XssProtectionOption.EnabledWithModeBlock:
                    return "1; mode=block";
                case XssProtectionOption.Enabled:
                    return "1";
                case XssProtectionOption.Disabled:
                    return "0";
                default:
                    throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
    }
}