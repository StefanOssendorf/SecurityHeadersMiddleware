using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecurityHeadersMiddleware.Infrastructure;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class PublicKeyPinningMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> PulibKeyPinningHeader(PublicKeyPinningConfiguration configuration) {
            return next => env => {
                var ctx = env.AsContext();

                // Only over secure transport (https://tools.ietf.org/html/rfc7469#section-2.2.2)
                // Quotation: "Pinned Hosts SHOULD NOT include the PKP header field in HTTP responses conveyed over non-secure transport." (06.07.2015)
                if (!ctx.Request.IsSecure) {
                    return next(env);
                }

                var resp = ctx.Response;
                var state = new State<PublicKeyPinningConfiguration>();
                resp.OnSendingHeaders(ApplyHeader, state);
                return next(env);
            };
        }

        private static void ApplyHeader(object obj) {
            
        }
    }

    //TODO Introduce new setting option to specify the behavior
    public class PublicKeyPinningConfiguration {
        private Dictionary<uint, PinTuple> mPins; 

        public ulong MaxAge { get; set; } // Required
        public bool IncludeSubDomains { get; set; } // Optional

        public Uri ReportUri { get; set; } // Optional

        public PublicKeyPinningConfiguration() {
            MaxAge = 0;
            IncludeSubDomains = false;
            ReportUri = null;
            mPins = new Dictionary<uint, PinTuple>();
        }

        public void AddPin(uint index, string pinValueNotEncoded, PinToken token) {
            mPins.Add(index, new PinTuple {
                Value = pinValueNotEncoded,
                Token = ToPinPrefix(token)
            });
        }

        private string ToPinPrefix(PinToken token) {
            switch (token) {
                case PinToken.Sha256:
                    return "sha256";
                default:
                    throw new ArgumentOutOfRangeException(nameof(token), token, null);
            }
        }


        private class PinTuple {
            public string Value;
            public string Token;
        }
    }

    public enum PinToken {
        Sha256
    }
}