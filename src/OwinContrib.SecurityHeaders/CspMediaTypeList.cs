using System;
using System.Collections.Generic;
using System.Linq;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    public class CspMediaTypeList {
        private readonly List<string> mMediaTypes;
        private static readonly List<string> mDiscreteTypes;


        static CspMediaTypeList() {
            mDiscreteTypes = new List<string> {
                "text", "image", "audio", "video", "application"
            };
        }
        public CspMediaTypeList() {
            mMediaTypes = new List<string>();
        }


        public void AddMediaType(string mediaType) {
            mediaType.MustNotBeWhiteSpaceOrEmpty("mediaType");

            VerifyMediaType(mediaType);
            mMediaTypes.Add(mediaType);
        }
        private void VerifyMediaType(string mediaType) {
            var split = mediaType.Split(new[] { "/" }, StringSplitOptions.None);
            if (split.Length != 2) {
                const string msg = "Mediatype value '{0}' does not satisfy the required format.{1}" +
                                   "Valid mediatypes: text/plain or text/html{1}" +
                                   "For more informatin see: {2} (media-type).";
                throw new FormatException(msg.FormatWith(mediaType, Environment.NewLine, "http://www.w3.org/TR/CSP2/#media-type-list1"));
            }

            VerifyType(split[0].ToLower());
            VeritySubType(split[1].ToLower());
        }
        private void VerifyType(string type) {
            if (mDiscreteTypes.Any(t => t == type)) {
                return;
            }
            if (TryVerifyAsXToken(type)) {
                return;
            }

            throw new NotImplementedException();
        }
        private bool TryVerifyAsXToken(string maybeXToken) {
            if (!maybeXToken.StartsWith("x-")) {
                return false;
            }


            return true;
        }
        private void VeritySubType(string subType) {
            throw new NotImplementedException();
        }
    }
}