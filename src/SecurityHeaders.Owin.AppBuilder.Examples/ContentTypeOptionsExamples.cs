using Owin;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class ContentTypeOptionsExamples {
        public void Example() {
            IAppBuilder buildFunc = null;

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            // Results in : X-Content-Type-Options: nosniff
            buildFunc.UseContentTypeOptions();

            // Use the X-Content-Type-Options middleware and do not set the header if already set.
            // Results in : X-Content-Type-Options: nosniff, if the header is not already set
            buildFunc.UseContentTypeOptions(
                settings => settings.IgnoreIfHeaderIsPresent()
            );
        }
    }
}
