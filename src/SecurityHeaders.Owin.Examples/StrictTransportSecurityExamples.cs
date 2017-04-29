using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantAssignment

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;
    public class StrictTransportSecurityExamples {
        public void Examples() {
            BuildFunc buildFunc = null;

            // Use Strict-Transport-Security middleware with default settings (Overwrite and max-age=31536000 and includeSubDomains)
            // Results in: Strict-Transport-Security: max-age=31536000; includeSubDomains
            buildFunc.StrictTransportSecurity();

            // Configure the settings via the fluent api.
            // This results in (depending on the choosen options) in combinations of the provided options.
            buildFunc.StrictTransportSecurity(
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