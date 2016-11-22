using Owin;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.AppBuilder.Examples {
    public class ContentTypeOptionsExamples {
        public void Example() {
            IAppBuilder buildFunc = null;

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            buildFunc.ContentTypeOptions();

            // Use the ContenTypeOptions middleware and do not set the header if already set.
            buildFunc.ContentTypeOptions(settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
        }
    }
}
