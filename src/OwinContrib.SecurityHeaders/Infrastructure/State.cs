using SecurityHeadersMiddleware.LibOwin;

namespace SecurityHeadersMiddleware.Infrastructure {
    internal class State<T> {
        public T Settings { get; set; }
        public IOwinResponse Response { get; set; }
    }
}