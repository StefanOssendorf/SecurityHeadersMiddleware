using System.Collections.Generic;
using System.ComponentModel;

namespace SecurityHeadersMiddleware {
    internal class ReferrerDirectiveValueBuilder : IDirectiveValueBuilder {
        private static readonly Dictionary<ReferrerKeyword, IDirectiveValueBuilder> Cache;

        private readonly ReferrerKeyword mKeyword;
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
                }
            };
        }
        private ReferrerDirectiveValueBuilder(ReferrerKeyword keyword) {
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
        internal static IDirectiveValueBuilder Get(ReferrerKeyword keyword) {
            return Cache[keyword];
        }
    }

    internal class ReflectedXssDirectiveValueBuilder : IDirectiveValueBuilder {
        private static readonly Dictionary<ReflectedXssKeyword, IDirectiveValueBuilder> Cache;
        private readonly ReflectedXssKeyword mKeyword;

        static ReflectedXssDirectiveValueBuilder() {
            Cache = new Dictionary<ReflectedXssKeyword, IDirectiveValueBuilder> {
                {
                    ReflectedXssKeyword.NotSet, new ReflectedXssDirectiveValueBuilder(ReflectedXssKeyword.NotSet)
                }, {
                    ReflectedXssKeyword.Block, new ReflectedXssDirectiveValueBuilder(ReflectedXssKeyword.Block)
                }, {
                    ReflectedXssKeyword.Allow, new ReflectedXssDirectiveValueBuilder(ReflectedXssKeyword.Allow)
                }, {
                    ReflectedXssKeyword.Filter, new ReflectedXssDirectiveValueBuilder(ReflectedXssKeyword.Filter)
                }
            };
        }
        private ReflectedXssDirectiveValueBuilder(ReflectedXssKeyword keyword) {
            mKeyword = keyword;
        }
        public string ToDirectiveValue() {
            switch (mKeyword) {
                case ReflectedXssKeyword.NotSet:
                    return "";
                default:
                    return mKeyword.ToString().ToLower();
            }
        }
        internal static IDirectiveValueBuilder Get(ReflectedXssKeyword word) {
            return Cache[word];
        }
    }
}