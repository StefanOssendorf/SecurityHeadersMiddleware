using Microsoft.Owin;

namespace OwinContrib.Security {
    public static class XssProtectionHeaderMiddleware {
        public static BuildFunc XssProtectionHeader(this BuildFunc builder) {
            builder(XssProtectionHeader());
            return builder;
        }
        public static MidFunc XssProtectionHeader() {
            return
                next =>
                    env => {
                        var context = env.AsContext();
                        context
                            .Response
                            .OnSendingHeaders(resp => { ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = "1; mode=block"; }, context.Response);
                        return next(env);
                    };
        }
    }
}