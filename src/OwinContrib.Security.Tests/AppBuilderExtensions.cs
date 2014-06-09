using Owin;

namespace OwinContrib.Security.Tests {
    internal static class AppBuilderExtensions {
        internal static BuildFunc Use(this IAppBuilder builder) {
            return middleware => builder.Use(middleware);
        }

        internal static IAppBuilder Use(this BuildFunc middleware, IAppBuilder builder) {
            return builder;
        }
    }
}