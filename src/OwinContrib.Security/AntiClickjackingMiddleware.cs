using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using OwinContrib.Security.Infrastructure;

namespace OwinContrib.Security {
    using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;
    public static class AntiClickjackingMiddleware {
        /// <summary>
        /// Adds the "X-Frame-Options" header with given option.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static MidFunc AntiClickjackingHeader(XFrameOption option) {
            Contract.Requires<ArgumentOutOfRangeException>(option == XFrameOption.Deny || option == XFrameOption.SameOrigin);
            return AntiClickjackingHeader((int)option, new Uri[0]);
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM value.
        /// </summary>
        /// <param name="origins"></param>
        /// <returns></returns>
        public static MidFunc AntiClickjackingHeader(params string[] origins) {
            Contract.Requires<ArgumentNullException>(origins != null);
            Contract.Requires<ArgumentOutOfRangeException>(origins.Length > 0);
            var uriOrigins = origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => new Uri(o)).ToArray();
            return AntiClickjackingHeader(uriOrigins);
        }
        /// <summary>
        /// Adds the "X-Frame-Options" with DENY when the request uri is not provided. Otherwise the request uri with ALLOW-FROM value.
        /// </summary>
        /// <param name="origins"></param>
        /// <returns></returns>
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