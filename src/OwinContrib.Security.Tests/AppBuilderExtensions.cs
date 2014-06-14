using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;

namespace OwinContrib.Security.Tests {
    using BuildFunc = Action<Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>;
    internal static class AppBuilderExtensions {
        internal static BuildFunc Use(this IAppBuilder builder) {
            return middleware => builder.Use(middleware);
        }

        internal static IAppBuilder Use(this BuildFunc middleware, IAppBuilder builder) {
            return builder;
        }
    }
}