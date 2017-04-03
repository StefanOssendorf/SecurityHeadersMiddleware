using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class XFrameOptionsExamples {    

        public void Example() {
            BuildFunc buildFunc = null;

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            // Results in: X-Frame-Options: DENY
            buildFunc.AntiClickjacking();

            // Choose the desired header-value
            var headerValue = XFrameOptionHeaderValue.Deny();
            headerValue = XFrameOptionHeaderValue.SameOrigin();
            headerValue = XFrameOptionHeaderValue.AllowFrom("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Frame-Options: DENY
            // - Results in: X-Frame-Options: SAMEORIGIN
            // - Results in: X-Frame-Options: ALLOW-FROM http://www.example.org
            buildFunc.AntiClickjacking(
                () =>
                    new AntiClickjackingSettings(headerValue,
                        AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}