using System;
using Microsoft.AspNetCore.Builder;
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void AntiClickJackingExamples(IApplicationBuilder app) {

            // Use X-Frame-Options middleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            app.UseXFrameOptions();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            app.UseXFrameOptions(
                 settings =>
                            //settings.Deny()
                            //settings.SameOrigin()
                            settings.AllowFrom(new Uri("http://www.example.org"))
                            .IgnoreIfHeaderIsPresent()
            );
        }
    }
}