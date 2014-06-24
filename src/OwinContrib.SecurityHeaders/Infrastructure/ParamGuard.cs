using System;

namespace SecurityHeadersMiddleware.Infrastructure {
    public static class ParamGuard {
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
    }
}