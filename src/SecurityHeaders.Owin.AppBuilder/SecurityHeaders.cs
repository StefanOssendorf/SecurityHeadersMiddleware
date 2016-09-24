using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace SecurityHeaders.Owin.AppBuilder {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public static partial class SecurityHeaders {
        internal static BuildFunc UseOwin(this IAppBuilder builder) {
            return middleware => builder.Use(middleware(builder.Properties));
        }
    }
}