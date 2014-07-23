using System;

namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class ParamGuard {
        public static void MustNotNull(this object source, string paramName) {
            if (source == null) {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void MustHaveAtLeastOneValue<T>(this T[] source, string paramName) {
            if (source.Length <= 0) {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        public static void MustNotBeWhiteSpaceOrEmpty(this string source, string paramName) {
            if (string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentException("{0} have to be not empty or only contains white-space characters.".FormatWith(paramName));
            }
        }
    }
}