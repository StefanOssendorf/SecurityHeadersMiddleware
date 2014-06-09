using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinContrib.Security {
    public delegate Task AppFunc(IDictionary<string, object> env);

    public delegate AppFunc MidFunc(AppFunc next);

    public delegate void BuildFunc(MidFunc builder);

    public static class XssProtectionHeaderMiddleware {
        public static BuildFunc XssProtectionHeader(this BuildFunc builder) {
            builder(XssProtectionHeader());
            return builder;
        }
        public static MidFunc XssProtectionHeader() {
            return
                next =>
                    env => {
                        var context = new OwinContext(env);
                        context
                            .Response
                            .OnSendingHeaders(resp => { ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = "1; mode=block"; }, context.Response);
                        return next(env);
                    };
        }
    }
}