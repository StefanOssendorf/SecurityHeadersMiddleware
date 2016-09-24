using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SecurityHeaders.AspNetCore {
    public static partial class SecurityHeaders {
        private static void OnStarting<T>(this HttpResponse source, Func<T, Task> callback, T context) {
            source.OnStarting(ctx => callback(context), context);
        }

        private static AspNetCoreHttpContext AsInternalCtx(this HttpContext source) {
            return new AspNetCoreHttpContext(source);
        }
    }
}