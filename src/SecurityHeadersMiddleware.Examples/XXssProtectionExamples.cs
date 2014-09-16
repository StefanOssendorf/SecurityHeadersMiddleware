using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class XXssProtectionExamples {
        public void Examples() {
            IAppBuilder appbuilder = null;
            BuildFunc buildFunc = null;

            // Add X-Xss-Protection: 1; mode=block
            buildFunc.XssProtectionHeader(); 
            appbuilder.XssProtectionHeader();

            // Add X-Xss-Protection: 0
            buildFunc.XssProtectionHeader(disabled: true);
            appbuilder.XssProtectionHeader(disabled: true);
        } 
    }
}