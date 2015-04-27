using System.Collections.Generic;
using System.Text;

namespace SecurityHeadersMiddleware {
    partial class CspSourceList {
        private readonly List<SourceListKeyword> mKeywords;

        /// <summary>
        ///     Adds a keyword to the source-list.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        public void AddKeyword(SourceListKeyword keyword) {
            ThrowIfNoneIsSet();
            if (mKeywords.Contains(keyword)) {
                return;
            }
            mKeywords.Add(keyword);
        }

        private string BuildKeywordValues() {
            var sb = new StringBuilder();
            foreach (var keyword in mKeywords) {
                var value = keyword.ToString().ToLower();
                if (value.StartsWith("unsafe")) {
                    value = value.Insert(6, "-");
                }
                sb.AppendFormat("'{0}' ", value);
            }
            return sb.ToString();
        }
    }
}