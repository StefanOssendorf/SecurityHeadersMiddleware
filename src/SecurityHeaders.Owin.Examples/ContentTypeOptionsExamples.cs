using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class ContentTypeOptionsExamples {
        public void Example() {
            BuildFunc buildFunc = null;

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            // Results in : X-Content-Type-Options: nosniff
            buildFunc.ContentTypeOptions();

            // Use the X-Content-Type-Options middleware and do not set the header if already set.
            // Results in : X-Content-Type-Options: nosniff, if the header is not already set
            buildFunc.ContentTypeOptions(
                settings => settings.IgnoreIfHeaderIsPresent()
            );
        }
    }
}