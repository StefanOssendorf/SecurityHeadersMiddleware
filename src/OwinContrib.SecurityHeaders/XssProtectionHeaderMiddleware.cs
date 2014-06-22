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
            return XssProtectionHeader(false);
        }
        /// <summary>
        /// Adds the "X-Xss-Protection" header depending on <paramref name="disabled"/>
        /// </summary>
        /// <param name="disabled">true to set the header value to "0". false for "1; mode=blick".</param>
        /// <returns></returns>
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> XssProtectionHeader(bool disabled) {
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