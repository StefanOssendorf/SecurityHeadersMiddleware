using System;
using Owin;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class XssProtectionExamples {
        public void Example() {
            IAppBuilder builder = null;

            // Use XssProtectionMiddleware with default settings (Overwrite and "1; mode=block")
            // Results in: X-Xss-Protection: 1; mode=block
            builder.UseXssProtection();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options):
            // - Results in: X-Xss-Protection: 0
            // - Results in: X-Xss-Protection: 1
            // - Results in: X-Xss-Protection: 1; mode=block
            // - Results in: X-Xss-Protection: 1; report=http://www.example.org
            builder.UseXssProtection(
                settings =>
                            //settings.Disabled()
                            //settings.Enabled()
                            //settings.EnabledAndBlock()
                            settings.EnabledAndReport(new Uri("http://www.example.org"))
                            .IgnoreIfHeaderIsPresent()
            );
        }
    }
}