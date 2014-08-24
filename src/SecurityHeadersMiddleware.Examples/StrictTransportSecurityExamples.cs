using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class StrictTransportSecurityExamples {
        public void Examples() {
            IAppBuilder appbuilder = null;
            BuildFunc buildFunc = null;

            // Add Strict-Transport-Security: max-age=31536000;includeSubDomains
            buildFunc.StrictTransportSecurity();
            appbuilder.StrictTransportSecurity();

            // Add Strict-Transport-Security with the configured settings
            var config = new StrictTransportSecurityOptions() {
                IncludeSubDomains = true,
                MaxAge = 31536000,
                RedirectToSecureTransport = true,
                // RedirectUriBuilder = uri => "";
                RedirectReasonPhrase = statusCode => "ResonPhrase"
            };
            buildFunc.StrictTransportSecurity(config);
            appbuilder.StrictTransportSecurity(config);
        }
    }
}