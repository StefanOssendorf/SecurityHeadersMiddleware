using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void AntiClickJackingExamples(IApplicationBuilder app) {
            // Choose the desired header-value
            var headerValue = XFrameOptionHeaderValue.Deny();
            headerValue = XFrameOptionHeaderValue.SameOrigin();
            headerValue = XFrameOptionHeaderValue.AllowFrom("http://www.example.org");

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            app.UseAntiClickjacking();

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