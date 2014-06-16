using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace OwinContrib.SecurityHeaders.Tests {
    internal static class AppBuilderExtensions {
        internal static Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> Use(this IAppBuilder builder) {
            return middleware => builder.Use(middleware);
        }

        internal static IAppBuilder Use(this Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>> middleware, IAppBuilder builder) {
            return builder;
        }
    }
}