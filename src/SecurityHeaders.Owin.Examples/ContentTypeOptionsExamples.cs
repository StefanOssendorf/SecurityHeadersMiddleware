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
            buildFunc.ContentTypeOptions();

            // Use the ContenTypeOptions middleware and do not set the header if already set.
            buildFunc.ContentTypeOptions(() => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}