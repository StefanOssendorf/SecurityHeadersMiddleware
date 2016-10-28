using Shouldly;
using Xunit;

namespace SecurityHeaders.Owin.Tests {
    public class OwinContextInternalTest {
        [Fact]
        public void When_adding_an_header_it_should_be_added() {
            var owinContext = new OwinContext();            
            var internalContext = new OwinContextInternal(owinContext);

            internalContext.AppendToHeader("Test", "Test");

            owinContext.Response.Headers.Keys.ShouldContain("Test");
            owinContext.Response.Headers["Test"].ShouldBe("Test");
        }

        [Fact]
        public void When_appending_to_a_header_it_should_be_appended() {
            var owinContext = new OwinContext();
            var internalContext = new OwinContextInternal(owinContext);
            owinContext.Response.Headers.Add("Test", new[] { "Abcd"});

            internalContext.AppendToHeader("Test", "Test");

            owinContext.Response.Headers.Keys.ShouldContain("Test");
            owinContext.Response.Headers["Test"].ShouldBe("Abcd,Test");
        }
    }
}