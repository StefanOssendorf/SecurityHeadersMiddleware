using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    public class CspMediaTypeList : IDirectiveValueBuilder {
        private readonly List<string> mMediaTypes;
        private const string TSpecial = "()<>@,;:\\\"/[]?= ";

        public CspMediaTypeList() {
            mMediaTypes = new List<string>();
        }


        public void AddMediaType(string mediaType) {
            mediaType.MustNotBeWhiteSpaceOrEmpty("mediaType");

            VerifyMediaType(mediaType);
            mMediaTypes.Add(mediaType);
        }
        private static void VerifyMediaType(string mediaType) {
            var split = mediaType.Split(new[] { "/" }, StringSplitOptions.None);
            if (split.Length != 2 || split[0].Length == 0 || split[1].Length == 0) {
                const string msg = "Mediatype value '{0}' does not satisfy the required format.{1}" +
                                   "Valid mediatypes: text/plain or text/html{1}" +
                                   "For more information see: {2} (media-type).";
                throw new FormatException(msg.FormatWith(mediaType, Environment.NewLine, "http://www.w3.org/TR/CSP2/#media-type-list1"));
            }

            VerifyType(split[0].ToLower(), mediaType);
            VeritySubType(split[1].ToLower(), mediaType);
        }
        private static void VerifyType(string type, string mediaType) {
            if (type.All(IsToken)) {
                return;
            }
            const string msg = "The type part '{0} of mediatype '{1}' does not satisfy the required format.{2}" +
                               "Type contains illegal characters.{2}" +
                               "For more information see: {3} (media-type).";
            throw new FormatException(msg.FormatWith(type, mediaType, Environment.NewLine, "http://www.w3.org/TR/CSP2/#media-type-list1"));
        }

        private static void VeritySubType(string subType, string mediaType) {
            if (subType.All(IsToken)) {
                return;
            }
            const string msg = "The subtype part '{0} of mediatype '{1}' does not satisfy the required format.{2}" +
                               "Subtype contains illegal characters.{2}" +
                               "For more information see: {3} (media-type).";
            throw new FormatException(msg.FormatWith(subType, mediaType, Environment.NewLine, "http://www.w3.org/TR/CSP2/#media-type-list1"));
        }

        private static bool IsToken(char value) {
            return value.IsAscii() && !value.IsCTL() && TSpecial.All(c => c != value);
        }

        public string ToDirectiveValue() {
            if (mMediaTypes.Count == 0) {
                return "";
            }
            var sb = new StringBuilder();
            foreach (var mediaType in mMediaTypes) {
                sb.AppendFormat(" {0} ", mediaType);
            }
            return sb.ToString();
        }
    }
}