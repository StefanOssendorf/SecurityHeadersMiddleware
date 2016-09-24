using System;

namespace SecurityHeaders.Core.Tests {
    internal class TestContext : IHttpContext {
        public Func<string, bool> HeaderExistFunc { get; set; }
        public Action<string, string> OverrideHeaderValueAction { get; set; }
        public Action<string, string> AppendHeaderValueAction { get; set; }

        public bool HeaderExist(string headerName) {
            return HeaderExistFunc(headerName);
        }

        public void OverrideHeader(string name, string value) {
            OverrideHeaderValueAction(name, value);
        }

        public void AppendToHeader(string name, string value) {
            AppendHeaderValueAction(name, value);
        }
    }
}