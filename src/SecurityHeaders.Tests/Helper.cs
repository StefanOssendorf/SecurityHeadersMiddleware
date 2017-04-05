using System;

namespace SecurityHeaders.Tests {
    public static class Helper {
        public static void IoExceptionThrower(string a) => throw new InvalidOperationException();
        public static void IoExceptionThrower(string a, string b) => throw new InvalidOperationException();
    }
}