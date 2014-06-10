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
                        var response = env.AsContext().Response;
                        response
                            .OnSendingHeaders(resp => { ((IOwinResponse)resp).Headers[HeaderConstants.XssProtection] = "1; mode=block"; }, response);
                        return next(env);
                    };
        }
    }
}