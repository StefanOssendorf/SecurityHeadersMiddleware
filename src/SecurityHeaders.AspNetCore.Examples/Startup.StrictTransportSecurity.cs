using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void StrictTransportSecurityExample(IApplicationBuilder app) {

            // Use Strict-Transport-Security middleware with default settings (Overwrite and max-age=31536000 and includeSubDomains)
            // Results in: Strict-Transport-Security: max-age=31536000; includeSubDomains
            app.UseStrictTransportSecurity();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options) in combinations of the provided options.
            app.UseStrictTransportSecurity(
                settings =>
                            settings.WithMaxAge(500)
                                  
                                    .IncludeSubdomains()
                                  //.NotIncludingSubdomains()
                                  
                                  //.WithoutPreload()
                                    .WithPreload()
                                  
                                    //.RedirectUnsecureToSecureRequests() // Upgrades to https
                                    .RedirectUnsecureToSecureRequests(urlToUpgrade => urlToUpgrade) // Provide a delegate to upgrade the uri from
                                                                                                    // an unsecure channel to a secure channel
                                  //.IgnoreIfHeaderIsPresent()
                                    .OverwriteHeaderIfHeaderIsPresent()
            );
        }
    }
}
