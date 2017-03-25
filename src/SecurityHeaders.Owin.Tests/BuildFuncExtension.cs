using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace SecurityHeaders.Owin.Tests {
    internal static class BuildFuncExtension {
        internal static Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>> UseOwin(this IAppBuilder builder) => middleware => builder.Use(middleware(builder.Properties));
    }
}