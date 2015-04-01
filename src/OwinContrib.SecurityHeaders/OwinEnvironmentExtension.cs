using System.Collections.Generic;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class OwinEnvironmentExtension {
        public static IOwinContext AsContext(this IDictionary<string, object> env) {
            return new OwinContext(env);
        }
    }
}