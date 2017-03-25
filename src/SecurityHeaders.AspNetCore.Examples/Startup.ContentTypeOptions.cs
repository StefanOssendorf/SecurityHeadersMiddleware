using Microsoft.AspNetCore.Builder;

namespace SecurityHeaders.AspNetCore.Examples {
    public partial class Startup {
        private static void ContentTypeOptionsExamples(IApplicationBuilder app) {
            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            // Results in : X-Content-Type-Options: nosniff
            app.UseContentTypeOptions();

            // Use the ContenTypeOptions middleware and do not set the header if already set.
            // Results in : X-Content-Type-Options: nosniff, if the header is not already set
            app.UseContentTypeOptions(
                () => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}