using System;

namespace SecurityHeaders.Core {
    internal static class Guard {
        public static void MustNotNull(this object source, string paramName) {
            if(source == null) {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws InvalidEnumArgumentException
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paramName"></param>
        public static void MustBeDefined(this Enum source, string paramName) {
            var enumType = source.GetType();
            if(!Enum.IsDefined(enumType, source)) {
                throw new ArgumentOutOfRangeException($"{source} is not a defined value on {enumType} (Parameter {paramName}");
            }
        }
    }
}