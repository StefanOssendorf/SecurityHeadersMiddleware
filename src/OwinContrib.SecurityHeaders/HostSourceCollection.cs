using System.Collections.ObjectModel;

namespace SecurityHeadersMiddleware {
    internal class HostSourceCollection : Collection<HostSource> {
        protected override void InsertItem(int index, HostSource item) {
            //TODO Contains necessary?
            if (Contains(item)) {
                return;
            }

            base.InsertItem(index, item);
        }
    }
}