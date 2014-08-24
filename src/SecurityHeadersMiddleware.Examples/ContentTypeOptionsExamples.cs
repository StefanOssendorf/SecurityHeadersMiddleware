using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;
using SecurityHeadersMiddleware.OwinAppBuilder;

namespace SecurityHeadersMiddleware.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class ContentTypeOptionsExamples {
        public void Examples() {
            IAppBuilder appbuilder = null;
            BuildFunc buildFunc = null;

            // Add X-Content-Type-Option: nosniff
            buildFunc.ContentTypeOptions();
            appbuilder.ContentTypeOptions();
        }
    }
}