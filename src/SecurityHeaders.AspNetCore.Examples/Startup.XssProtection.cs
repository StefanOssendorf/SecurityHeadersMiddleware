using System;
using Microsoft.AspNetCore.Builder;
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private void XssProtection(IApplicationBuilder app) {
            // Use XssProtectionMiddleware with default settings (Overwrite and "1; mode=block")
            // Results in: X-Xss-Protection: 1; mode=block
            app.UseXssProtection();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options):
            // - Results in: X-Xss-Protection: 0
            // - Results in: X-Xss-Protection: 1
            // - Results in: X-Xss-Protection: 1; mode=block
            // - Results in: X-Xss-Protection: 1; report=http://www.example.org
            app.UseXssProtection(
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