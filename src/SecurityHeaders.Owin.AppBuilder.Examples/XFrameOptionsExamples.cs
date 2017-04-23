using Owin;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class XFrameOptionsExamples {

        public void Example() {
            IAppBuilder buildFunc = null;

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            buildFunc.UseAntiClickjacking();

            // Choose the desired header-value
            var headerValue = AntiClickjackingHeaderValue.Deny();
            headerValue = AntiClickjackingHeaderValue.SameOrigin();
            headerValue = AntiClickjackingHeaderValue.AllowFrom("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            buildFunc.UseAntiClickjacking(() => new AntiClickjackingSettings(headerValue, AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}