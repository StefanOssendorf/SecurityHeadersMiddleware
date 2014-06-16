using System.Collections.Generic;
using Microsoft.Owin;

namespace OwinContrib.SecurityHeaders {
    internal static class OwinEnvironmentExtension {
        public static IOwinContext AsContext(this IDictionary<string, object> env) {
            return new OwinContext(env);
        }
    }
}