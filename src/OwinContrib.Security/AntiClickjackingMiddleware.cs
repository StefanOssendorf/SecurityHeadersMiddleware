using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinContrib.Security {

    public static class AntiClickjackingMiddleware {

        public static MidFunc AntiClickjackingHeader() {
            return next =>
                env => {
                    var context = env.AsContext();
                    context
                        .Response
                        .OnSendingHeaders(ApplyHeader, context.Response);
                    return next(env);
                };
        }
        private static void ApplyHeader(object obj) {
            var response = (IOwinResponse)obj;
        }
    }


    internal class AntiClickjackingMiddleware2 : OwinMiddleware {
        private const string XFrameOptionsHeaderName = "X-Frame-Options";
        private readonly List<Uri> mAllowedOrigins;
        private readonly XFrameOption mOption;

        public AntiClickjackingMiddleware2(OwinMiddleware next, XFrameOption option, params string[] origins)
            : base(next) {
            mOption = option;
            mAllowedOrigins = new List<Uri>();
            if (origins == null || origins.Length == 0) {
                return;
            }
            mAllowedOrigins = origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new Uri(o)).ToList();
        }
        public override async Task Invoke(IOwinContext context) {
            context.Response.OnSendingHeaders(cont => OnResponseSending((OwinContext)cont), context);
            await Next.Invoke(context);
        }
        private void OnResponseSending(IOwinContext context) {
            IOwinResponse response = context.Response;
            string value;
            switch (mOption) {
                case XFrameOption.Deny:
                case XFrameOption.SameOrigin:
                    value = mOption.ToString().ToUpper();
                    break;
                default:
                    Uri uri = mAllowedOrigins.Find(u => Rfc6454Utility.HasSameOrigin(u, context.Request.Uri));
                    if (uri == null) {
                        value = XFrameOption.Deny.ToString().ToUpper();
                    } else {
                        value = "ALLOW-FROM {0}".Formatted(Rfc6454Utility.SerializeOrigin(uri));
                    }
                    break;
            }
            response.Headers[XFrameOptionsHeaderName] = value;
        }
    }
}