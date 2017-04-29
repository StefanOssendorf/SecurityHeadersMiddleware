using System;

namespace SecurityHeaders {
    internal static class Guard {
        public static void NotNull(object source, string paramName) {
            if(source == null) {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNullOrWhiteSpace(string source, string paramName) {
            NotNull(source, paramName);
            if(string.IsNullOrWhiteSpace(source)) {
                throw new ArgumentException("The string must not be null, empty or whitespace.", paramName);
            }
        }

        /// <summary>
        /// Throws InvalidEnumArgumentException
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramName"></param>
        public static void MustBeDefined(Enum source, string paramName) {
            var enumType = source.GetType();
            if(!Enum.IsDefined(enumType, source)) {
                throw new ArgumentOutOfRangeException($"{source} is not a defined value on {enumType} (Parameter {paramName}");
            }
        }

        public static void AtLeast(int value, int minValue, string paramName) {
            if(value < minValue) {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value of {paramName} is less than {minValue}. But it must be at least {minValue}.");
            }
        }
    }


    // Temporary
    internal static class Headers {
        public const string StrictTransportSecurity = "Strict-Transport-Security";
        public const string Location = "Location";

        public const string ContentSecurityPolicy = "Content-Security-Policy";
        public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";
        public const string PublicKeyPinning = "Public-Key-Pins";

    }
}