using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace OwinContrib.Security {
    public static class AntiClickjackingMiddleware {
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder) {
            builder(AntiClickjackingHeader(XFrameOption.Deny));
            return builder;
        }
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, XFrameOption option) {
            builder(AntiClickjackingHeader(option));
            return builder;
        }
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, params string[] origins) {
            builder(AntiClickjackingHeader(origins));
            return builder;
        }
        public static BuildFunc AntiClickjackingHeader(this BuildFunc builder, params Uri[] origins) {
            builder(AntiClickjackingHeader(origins));
            return builder;
        }



        public static MidFunc AntiClickjackingHeader(XFrameOption option) {
            Contract.Requires<ArgumentOutOfRangeException>(option == XFrameOption.Deny || option == XFrameOption.SameOrigin);
            return AntiClickjackingHeader((int)option, new Uri[0]);
        }
        public static MidFunc AntiClickjackingHeader(params string[] origins) {
            Contract.Requires<ArgumentNullException>(origins != null);
            Contract.Requires<ArgumentOutOfRangeException>(origins.Length > 0);
            var uriOrigins = origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new Uri(o)).ToArray();
            return AntiClickjackingHeader(uriOrigins);
        }
        public static MidFunc AntiClickjackingHeader(params Uri[] origins) {
            Contract.Requires<ArgumentNullException>(origins != null);
            Contract.Requires<ArgumentOutOfRangeException>(origins.Length > 0);
            return AntiClickjackingHeader(3, origins);
        }

        internal static MidFunc AntiClickjackingHeader(int option, params Uri[] origins) {
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