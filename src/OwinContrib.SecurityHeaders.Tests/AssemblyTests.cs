using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class AssemblyTests {
        [Fact]
        public void SecurityHeadersMiddlewareAssembly_should_not_reference_owin_assembly() {
            var assembly = Assembly.GetAssembly(typeof (ContentSecurityPolicyMiddleware));
            var referencedAssemblies = assembly.GetReferencedAssemblies();
            referencedAssemblies.Should().NotContain(dll => dll.Name == "SecurityHeadersMiddleware.OwinAppBuilder", "SecurityHeadersMiddleware should not reference SecurityHeadersMiddleware.OwinAppBuilder");
        }
    }
}