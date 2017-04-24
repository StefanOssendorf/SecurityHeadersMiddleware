using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class XFrameOptionsExamples {    

        public void Example() {
            BuildFunc buildFunc = null;

            // Use X-Frame-Options middleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            buildFunc.XFrameOptions();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            buildFunc.XFrameOptions(
                settings =>
                            //settings.Deny()
                            //settings.SameOrigin()
                            settings.AllowFrom(new Uri("http://www.example.org"))
                            .IgnoreIfHeaderIsPresent()
            );
        }
    }
}