using System;

namespace SecurityHeaders.Core.Tests {
    internal class TestContext : IHttpContext {
        public Func<string, bool> HeaderExistFunc { get; set; }
        public Action<string, string> OverrideHeaderValueAction { get; set; }
        public Action<string, string> AppendHeaderValueAction { get; set; }

        public bool HeaderExist(string headerName) {
            return HeaderExistFunc(headerName);
        }

        public void OverrideHeader(string headerName, string value) {
            OverrideHeaderValueAction(headerName, value);
        }

        public void AppendToHeader(string headerName, string value) {
            AppendHeaderValueAction(headerName, value);
        }
    }
}