using Microsoft.AspNetCore.Builder;
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void AntiClickJackingExamples(IApplicationBuilder app) {

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            app.UseAntiClickjacking();

            // Choose the desired header-value
            var headerValue = AntiClickjackingHeaderValue.Deny();
            headerValue = AntiClickjackingHeaderValue.SameOrigin();
            headerValue = AntiClickjackingHeaderValue.AllowFrom("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            app.UseAntiClickjacking(
                () =>
                    new AntiClickjackingSettings(headerValue,
                        AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}