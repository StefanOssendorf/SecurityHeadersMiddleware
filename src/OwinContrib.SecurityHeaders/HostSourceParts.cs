namespace SecurityHeadersMiddleware {
    internal class HostSourceParts {
        public HostSourceParts() {
            Scheme = "";
            Host = "";
            Port = "";
            Path = "";
        }

        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
    }
}