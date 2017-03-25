using Owin;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class ContentTypeOptionsExamples {
        public void Example() {
            IAppBuilder buildFunc = null;

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            // Results in : X-Content-Type-Options: nosniff
            buildFunc.ContentTypeOptions();

            // Use the ContenTypeOptions middleware and do not set the header if already set.
            // Results in : X-Content-Type-Options: nosniff, if the header is not already set
            buildFunc.ContentTypeOptions(() => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}
