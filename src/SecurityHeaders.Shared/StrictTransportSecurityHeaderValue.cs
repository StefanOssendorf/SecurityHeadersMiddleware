namespace SecurityHeaders {
    internal class StrictTransportSecurityHeaderValue {
        public string HeaderValue { get; }

        private StrictTransportSecurityHeaderValue(string value) {
            HeaderValue = value;
        }

        internal static StrictTransportSecurityHeaderValue Create(uint maxAge, bool includeSubdomains, bool preload) {
            string value = $"max-age={maxAge}";

            if(includeSubdomains) {
                value += "; includeSubDomains";
            }

            if(preload) {
                value += "; preload";
            }

            return new StrictTransportSecurityHeaderValue(value);
        }

        public static implicit operator string(StrictTransportSecurityHeaderValue headerValue) {
            return headerValue.HeaderValue;
        }
    }
}
