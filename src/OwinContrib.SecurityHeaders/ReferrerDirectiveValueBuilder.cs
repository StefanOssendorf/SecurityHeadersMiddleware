using System.Collections.Generic;
using System.ComponentModel;

namespace SecurityHeadersMiddleware {
    internal class ReferrerDirectiveValueBuilder : IDirectiveValueBuilder {
        private static readonly Dictionary<ReferrerKeyword, IDirectiveValueBuilder> Cache;

        static ReferrerDirectiveValueBuilder() {
            Cache = new Dictionary<ReferrerKeyword, IDirectiveValueBuilder> {
                {
                    ReferrerKeyword.NotSet, new ReferrerDirectiveValueBuilder(ReferrerKeyword.NotSet)
                }, {
                    ReferrerKeyword.None, new ReferrerDirectiveValueBuilder(ReferrerKeyword.None)
                }, {
                    ReferrerKeyword.NoneWhenDowngrade, new ReferrerDirectiveValueBuilder(ReferrerKeyword.NoneWhenDowngrade)
                }, {
                    ReferrerKeyword.Origin, new ReferrerDirectiveValueBuilder(ReferrerKeyword.Origin)
                }, {
                    ReferrerKeyword.OriginWhenCossOrigin, new ReferrerDirectiveValueBuilder(ReferrerKeyword.OriginWhenCossOrigin)
                }, {
                    ReferrerKeyword.UnsafeUrl, new ReferrerDirectiveValueBuilder(ReferrerKeyword.UnsafeUrl)
                } };
        }

        internal static IDirectiveValueBuilder Get(ReferrerKeyword keyword) {
            return Cache[keyword];
        }

        private readonly ReferrerKeyword mKeyword;
        public ReferrerDirectiveValueBuilder(ReferrerKeyword keyword) {
            mKeyword = keyword;
        }
        public string ToDirectiveValue() {
            switch (mKeyword) {
                case ReferrerKeyword.NotSet:
                    return "";
                case ReferrerKeyword.None:
                    return "none";
                case ReferrerKeyword.NoneWhenDowngrade:
                    return "none-when-downgrade";
                case ReferrerKeyword.Origin:
                    return "origin";
                case ReferrerKeyword.OriginWhenCossOrigin:
                    return "origin-when-cross-origin";
                case ReferrerKeyword.UnsafeUrl:
                    return "unsafe-url";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}