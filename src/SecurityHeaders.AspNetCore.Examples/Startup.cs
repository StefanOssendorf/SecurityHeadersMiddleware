using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SecurityHeaders.AspNetCore.Examples {
    

    // For exmaples please look at the other files of this class. 
    // E.g. Startup.ContentTypeOptions.cs for the ContentTypeOptions examples.

    public partial class Startup {
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
            XssProtection(app);
            StrictTransportSecurityExample(app);

            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
