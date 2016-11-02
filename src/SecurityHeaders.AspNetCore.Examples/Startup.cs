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


            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static void ContentTypeOptionsExamples(IApplicationBuilder app) {
            // Use the ContenTypeOptions middleware and do not set the header if already set.
            app.UseContentTypeOptions(settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);

            // Default sets HeadeHandling to ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet
            app.UseContentTypeOptions();
        }
    }
}
