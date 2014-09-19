using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class AntiClickjackingExamples {
        public void Examples() {
            IAppBuilder appbuilder = null;
            BuildFunc buildFunc = null;

            // Add X-Frame-Options: DENY
            buildFunc.AntiClickjackingHeader();
            appbuilder.AntiClickjackingHeader();

            // Add X-Frame-Options: SAMEORIGIN
            buildFunc.AntiClickjackingHeader(XFrameOption.SameOrigin);
            appbuilder.AntiClickjackingHeader(XFrameOption.SameOrigin);

            // Add X-Frame-Options: ALLOW-FROM http://www.exmple.com   when the Request uri is the allow-from uri.
            // Otherwise DENY will be sent.
            buildFunc.AntiClickjackingHeader("http://www.example.com", "https://www.example.com");
            appbuilder.AntiClickjackingHeader("http://www.example.com", "https://www.example.com");
            // or with URIs
            buildFunc.AntiClickjackingHeader(new Uri("http://www.example.com"), new Uri("https://www.example.com"));
            appbuilder.AntiClickjackingHeader(new Uri("http://www.example.com"), new Uri("https://www.example.com"));
        }
    }
}