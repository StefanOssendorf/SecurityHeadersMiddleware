using System;
using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware {
    internal static class IOwinResponseExtension {
        public static void OnSendingHeaders<T>(this IOwinResponse source, Action<T> callback, T state) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            Action<object> innerCallback = innerState => callback((T)innerState);
            source.OnSendingHeaders(innerCallback, state);
        }
    }
}