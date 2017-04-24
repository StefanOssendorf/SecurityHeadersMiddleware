#if ASPNETCORE
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using SecurityHeaders.AspNetCore;
using SecurityHeaders.Builders;

namespace SecurityHeaders.Tests {

    static partial class TestHttpClientFactory {
        private static HttpClient CreateCoreCto(Action<IFluentCtoSettingsBuilder> settingsBuilder = null, string headerValue = null) {
            Action<HttpContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Add("X-Content-Type-Options", headerValue);
            }
            return CreateCoreBased(app => {
                if(settingsBuilder == null) {
                    app.UseContentTypeOptions();
                } else {
                    app.UseContentTypeOptions(settingsBuilder);
                }
            }, maybeAddHeader
            );
        }

        private static HttpClient CreateCoreXfo(Action<IFluentXfoSettingsBuilder> settingsBuilder, string headerValue) {
            Action<HttpContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Add("X-Frame-Options", headerValue);
            }
            return CreateCoreBased(app => {
                if(settingsBuilder == null) {
                    app.UseXFrameOptions();
                } else {
                    app.UseXFrameOptions(settingsBuilder);
                }
            }, maybeAddHeader
            );
        }

        private static HttpClient CreateCoreXp(Action<IFluentXpSettingsBuilder> settingsBuilder, string headerValue) {
            Action<HttpContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Add("X-Xss-Protection", headerValue);
            }
            return CreateCoreBased(app => {
                if(settingsBuilder == null) {
                    app.UseXssProtection();
                } else {
                    app.UseXssProtection(settingsBuilder);
                }
            }, maybeAddHeader
            );
        }

        private static HttpClient CreateCoreBased(Action<IApplicationBuilder> addMiddleware, Action<HttpContext> modifyEndpoint = null) {
            var builder = new WebHostBuilder()
                .Configure(app => {
                    addMiddleware(app);
                    app.Run(async ctx => {
                        ctx.Response.StatusCode = 200;
                        modifyEndpoint?.Invoke(ctx);
                        await ctx.Response.WriteAsync("Hello World!");
                    });
                });
            return new TestServer(builder).CreateClient();
        }
    }
}
#endif