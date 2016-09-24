namespace SecurityHeaders {
    internal interface IHttpContext {
        bool HeaderExist(string headerName);
        void OverrideHeader(string name, string value);
        void AppendToHeader(string name, string value);
    }
}