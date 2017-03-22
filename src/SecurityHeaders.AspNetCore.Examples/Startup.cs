using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SecurityHeaders.AspNetCore.Examples {
    

    public class Startup {
        public static void Main() {
            new WebHostBuilder()
                .UseKestrel()
                .Configure(a => a.Run(c => c.Response.WriteAsync("Hi!")))
                .Build()
                .Run();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            ContentTypeOptionsExamples(app);
            AntiClickJackingExamples(app);

            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static void ContentTypeOptionsExamples(IApplicationBuilder app) {
            // Use the ContenTypeOptions middleware and do not set the header if already set.
            app.UseContentTypeOptions(() => new ContentTypeOptionsSettings(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet));

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            app.UseContentTypeOptions();
        }

        private static void AntiClickJackingExamples(IApplicationBuilder app) {
            // Choose the desired header-value
            var headerValue = XFrameOptionHeaderValue.Deny();
            headerValue = XFrameOptionHeaderValue.SameOrigin();
            headerValue = XFrameOptionHeaderValue.AllowFrom("http://www.example.org");

            // Create a settings and pass the apropriate values to the constructor
            app.UseAntiClickjacking(() => new AntiClickjackingSettings(headerValue, AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet));

            // Use AntiClickjackingMiddleware with default settings (Overwrite and DENY)
            app.UseAntiClickjacking();
        }
    }
}
