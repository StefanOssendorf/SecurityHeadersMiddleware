using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OwinContrib.SecurityHeaders.Infrastructure;

namespace OwinContrib.SecurityHeaders {
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;

    internal static class AntiClickjackingMiddleware {
        public static MidFunc AntiClickjackingHeader(XFrameOption option) {
            return AntiClickjackingHeader((int)option, new Uri[0]);
        }
        public static MidFunc AntiClickjackingHeader(params string[] origins) {
            var uriOrigins = origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new Uri(o)).ToArray();
            return AntiClickjackingHeader(uriOrigins);
        }
        public static MidFunc AntiClickjackingHeader(params Uri[] origins) {
            return AntiClickjackingHeader(3, origins);
        }

        public static MidFunc AntiClickjackingHeader(int option, params Uri[] origins) {
            return next =>
                env => {
                    var context = env.AsContext();
                    var response = context.Response;
                    var options = new {
                        FrameOption = option,
                        Origins = origins,
                        Response = response,
                        RequestUri = context.Request.Uri
                    };
                    response.OnSendingHeaders(ApplyHeader, options);
                    return next(env);
                };
        }
        private static void ApplyHeader(dynamic obj) {
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
            var uri = Array.Find(origins, u => Rfc6454Utility.HasSameOrigin(u, requestUri));
            return uri == null ? "DENY" : "ALLOW-FROM {0}".FormatWith(Rfc6454Utility.SerializeOrigin(uri));
        }
    }
}