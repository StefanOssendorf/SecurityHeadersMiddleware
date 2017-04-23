using System;
using System.Net.Http;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Tests {
    internal static partial class TestHttpClientFactory {

        public static HttpClient CreateCto(Action<IFluentCtoSettingsBuilder> settingsBuilder = null, string headerValue = null) {
#if ASPNETCORE
            return CreateCoreCto(settingsBuilder, headerValue);
#else
            return CreateOwinCto(settingsBuilder, headerValue);
#endif
        }

        public static HttpClient CreateXfo(Action<IFluentXfoSettingsBuilder> settingsBuilder = null, string headerValue = null) {
#if ASPNETCORE
            return CreateCoreXfo(settingsBuilder, headerValue);
#else
            return CreateOwinXfo(settingsBuilder, headerValue);
#endif
        }

        public static HttpClient CreateXp(Action<IFluentXpSettingsBuilder> settingsBuilder = null, string headerValue = null) {
#if ASPNETCORE
            return CreateCoreXp(settingsBuilder, headerValue);
#else
            return CreateOwinXp(settingsBuilder, headerValue);
#endif
        }

#if ASPNETCORE
        private static HttpClient CreateClient(Action<IApplicationBuilder> addMiddleware, Action<HttpContext> modifyRun = null) {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    addMiddleware(app);
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        modifyRun?.Invoke(ctx);
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            return new TestServer(builder).CreateClient();
        }
#else
        private static HttpClient CreateClient() {
            return new HttpClient();
        }
#endif
    }
}
