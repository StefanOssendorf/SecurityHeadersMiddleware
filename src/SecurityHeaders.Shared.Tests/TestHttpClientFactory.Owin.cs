#if OWIN
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;
using SecurityHeaders.Builders;
using SecurityHeaders.Owin;


namespace SecurityHeaders.Tests {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    internal static partial class TestHttpClientFactory {

        private static HttpClient CreateOwinCto(Action<IFluentCtoSettingsBuilder> settingsBuilder = null, string headerValue = null) {
            Action<IOwinContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Set("X-Content-Type-Options", headerValue);
            }
            return CreateOwinBased(buildFunc => {
                if(settingsBuilder == null) {
                    buildFunc.ContentTypeOptions();
                } else {
                    buildFunc.ContentTypeOptions(settingsBuilder);
                }
                }, maybeAddHeader
            );
        }

        private static HttpClient CreateOwinXfo(Action<IFluentXfoSettingsBuilder> settingsBuilder, string headerValue) {
            Action<IOwinContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Set("X-Frame-Options", headerValue);
            }
            return CreateOwinBased(buildFunc => {
                if(settingsBuilder == null) {
                    buildFunc.XFrameOptions();
                } else {
                    buildFunc.XFrameOptions(settingsBuilder);
                }
            }, maybeAddHeader
            );
        }

        private static HttpClient CreateOwinXp(Action<IFluentXpSettingsBuilder> settingsBuilder, string headerValue) {
            Action<IOwinContext> maybeAddHeader = null;
            if(headerValue != null) {
                maybeAddHeader = ctx => ctx.Response.Headers.Set("X-Xss-Protection", headerValue);
            }
            return CreateOwinBased(buildFunc => {
                if(settingsBuilder == null) {
                    buildFunc.XssProtection();
                } else {
                    buildFunc.XssProtection(settingsBuilder);
                }
            }, maybeAddHeader
            );
        }

            private static HttpClient CreateOwinBased(Action<BuildFunc> addMiddleware, Action<IOwinContext> modifyEndpoint = null) {
            var app = new AppBuilder();
            addMiddleware(app.UseOwin());
            app.Use((context, next) => {
                context.Response.StatusCode = 200;
                context.Response.ReasonPhrase = "OK";
                modifyEndpoint?.Invoke(context);
                return Task.FromResult(0);
            });
            var appFunc = app.Build();

            return new HttpClient(new OwinHttpMessageHandler(appFunc));
        }
    }

    internal static class BuildFuncExtension {
        internal static BuildFunc UseOwin(this IAppBuilder builder) => middleware => builder.Use(middleware(builder.Properties));
    }
}
#endif
