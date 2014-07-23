namespace SecurityHeadersMiddleware {
    internal class HostSourceParts {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }

        public HostSourceParts() {
            Scheme = "";
            Host = null;
            Port = "";
            Path = "";
        }
    }
}