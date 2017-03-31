using Shouldly;
using Xunit;

namespace SecurityHeaders.Owin.Tests {
    public class OwinHttpContextTest {
        [Fact]
        public void When_append_to_a_header_and_header_is_not_present_it_should_be_added() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);

            internalContext.AppendToHeader("Test", "Test");

            owinContext.Response.Headers.Keys.ShouldContain("Test");
            owinContext.Response.Headers["Test"].ShouldBe("Test");
        }

        [Fact]
        public void When_appending_to_a_header_it_should_be_appended() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);
            owinContext.Response.Headers.Add("Test", new[] { "Abcd" });

            internalContext.AppendToHeader("Test", "Test");

            owinContext.Response.Headers["Test"].ShouldBe("Abcd,Test");
        }

        [Fact]
        public void When_checking_if_an_absent_header_exist_it_should_return_false() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);

            internalContext.HeaderExist("Test").ShouldBeFalse();
        }

        [Fact]
        public void When_checking_if_a_present_header_exist_it_should_return_true() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);
            owinContext.Response.Headers.Add("Test", new []{"Test"});

            internalContext.HeaderExist("Test").ShouldBeTrue();
        }

        [Fact]
        public void When_overriding_an_absent_header_it_should_be_added() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);

            internalContext.OverrideHeader("Test", "Test");

            owinContext.Response.Headers.Keys.ShouldContain("Test");
            owinContext.Response.Headers["Test"].ShouldBe("Test");
        }

        [Fact]
        public void When_overriding_a_header_the_value_should_be_overridden() {
            var owinContext = new OwinContext();
            var internalContext = new OwinHttpContext(owinContext.Environment);
            owinContext.Response.Headers.Add("Test", new[] { "Test" });

            internalContext.OverrideHeader("Test", "krznbf");

            owinContext.Response.Headers["Test"].ShouldBe("krznbf");
        }
    }
}