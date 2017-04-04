using System;

namespace SecurityHeaders.Tests {
    public static class Helper {
        public static readonly Action<string, string> IoExceptionThrower = (a, b) => throw new InvalidOperationException();
    }
}