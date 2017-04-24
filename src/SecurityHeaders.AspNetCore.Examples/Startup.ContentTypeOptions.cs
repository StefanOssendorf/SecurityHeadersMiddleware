using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void ContentTypeOptionsExamples(IApplicationBuilder app) {
            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            // Results in : X-Content-Type-Options: nosniff
            app.UseContentTypeOptions();

            // Use the X-Content-Type-Options middleware and do not set the header if already set.
            // Results in : X-Content-Type-Options: nosniff, if the header is not already set
            app.UseContentTypeOptions(
                settings => settings.IgnoreIfHeaderIsPresent()
            );
        }
    }
}