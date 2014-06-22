using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace OwinContrib.SecurityHeaders.Tests {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    internal static class AppBuilderExtensions {
        public static BuildFunc UseOwin(this IAppBuilder builder) {
            return middleware => builder.Use(middleware(builder.Properties));
        }
    }
}