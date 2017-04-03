using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable ExpressionIsAlwaysNull

namespace SecurityHeaders.Owin.Examples {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public class XssProtectionExamples {
        public void Example() {
            BuildFunc buildFunc = null;

            // Use XssProtectionMiddleware with default settings (Overwrite and "1; mode=block")
            // Results in: X-Xss-Protection: 1; mode=block
            buildFunc.XssProtection();

            // Choose the desired header-value
            var headerValue = XssProtectionHeaderValue.Disabled();
            headerValue = XssProtectionHeaderValue.Enabled();
            headerValue = XssProtectionHeaderValue.EnabledAndBlock();
            headerValue = XssProtectionHeaderValue.EnabledAndReport("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            // Results in (depending on the choosen header value):
            // - Results in: X-Xss-Protection: 0
            // - Results in: X-Xss-Protection: 1
            // - Results in: X-Xss-Protection: 1; mode=block
            // - Results in: X-Xss-Protection: 1; report=http://www.example.org
            buildFunc.XssProtection(
                () =>
                    new XssProtectionSettings(headerValue,
                        XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet));
        }
    }
}