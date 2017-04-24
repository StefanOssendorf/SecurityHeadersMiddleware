using System;
using Owin;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class XFrameOptionsExamples {

        public void Example() {
            IAppBuilder buildFunc = null;

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