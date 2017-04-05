using System;

namespace SecurityHeaders.Tests {
    internal class TestContext : IHttpContext {
        public Func<string, bool> HeaderExistFunc { get; set; }
        public Action<string, string> OverrideHeaderValueAction { get; set; } = Helper.IoExceptionThrower;
        public Action<string, string> AppendHeaderValueAction { get; set; } = Helper.IoExceptionThrower;
        public Action<string> PermanentRedirectToAction { get; set; } = Helper.IoExceptionThrower;

        public bool HeaderExist(string headerName) {
            return HeaderExistFunc(headerName);
        }

        public void OverrideHeader(string headerName, string value) {
            OverrideHeaderValueAction(headerName, value);
        }

        public void AppendToHeader(string headerName, string value) {
            AppendHeaderValueAction(headerName, value);
        }

        public bool IsSecure { get; set; }

        public Uri RequestUri { get; set; }

        public void PermanentRedirectTo(Uri redirectedTo) {
            PermanentRedirectToAction(redirectedTo.ToString());
        }
    }
}