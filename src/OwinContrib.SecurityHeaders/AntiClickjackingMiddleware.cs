using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class AntiClickjackingMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> AntiClickjackingHeader(XFrameOption option) {
            return AntiClickjackingHeader((int)option, new Uri[0]);
        }
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> AntiClickjackingHeader(params string[] origins) {
            Uri[] uriOrigins = origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new Uri(o)).ToArray();
            return AntiClickjackingHeader(uriOrigins);
        }
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> AntiClickjackingHeader(params Uri[] origins) {
            return AntiClickjackingHeader(3, origins);
        }

        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> AntiClickjackingHeader(int option, params Uri[] origins) {
            return next =>
                env => {
                    IOwinContext context = env.AsContext();
                    IOwinResponse response = context.Response;
                    var options = new HeaderOptions {
                        FrameOption = option,
                        Origins = origins,
                        Response = response,
                        RequestUri = context.Request.Uri
                    };
                    response.OnSendingHeaders(ApplyHeader, options);
                    return next(env);
                };
        }
        private static void ApplyHeader(HeaderOptions obj) {
            string value = "";
            switch ((int)obj.FrameOption) {
                case 1:
                    value = "DENY";
                    break;
                case 2:
                    value = "SAMEORIGIN";
                    break;
                default:
                    value = DetermineValue((Uri[])obj.Origins, (Uri)obj.RequestUri);
                    break;
            }
            obj.Response.Headers[HeaderConstants.XFrameOptions] = value;
        }
        private static string DetermineValue(Uri[] origins, Uri requestUri) {
            Uri uri = Array.Find(origins, u => Rfc6454Utility.HasSameOrigin(u, requestUri));
            return uri == null ? "DENY" : "ALLOW-FROM {0}".FormatWith(Rfc6454Utility.SerializeOrigin(uri));
        }

        private class HeaderOptions {
            public int FrameOption { get; set; }
            public Uri[] Origins { get; set; }
            public IOwinResponse Response { get; set; }
            public Uri RequestUri { get; set; }
        }
    }
}