using Owin;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class XssProtectionExamples {
        public void Example() {
            IAppBuilder buildFunc = null;

            // Use XssProtectionMiddleware with default settings (Overwrite and "1; mode=block")
            // Results in: X-Xss-Protection: 1; mode=block
            buildFunc.UseXssProtection();

            // Choose the desired header-value
            var headerValue = XssProtectionHeaderValue.Disabled();
            headerValue = XssProtectionHeaderValue.Enabled();
            headerValue = XssProtectionHeaderValue.EnabledAndBlock();
            headerValue = XssProtectionHeaderValue.EnabledAndReport("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Xss-Protection: 0
            // - Results in: X-Xss-Protection: 1
            // - Results in: X-Xss-Protection: 1; mode=block
            // - Results in: X-Xss-Protection: 1; report=http://www.example.org
            buildFunc.UseXssProtection(
                () =>
                    new XssProtectionSettings(headerValue,
                        XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}