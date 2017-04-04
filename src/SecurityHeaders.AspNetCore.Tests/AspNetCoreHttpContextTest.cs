using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace SecurityHeaders.AspNetCore.Tests {
    public class AspNetCoreHttpContextTest {
        [Fact]
        public void When_append_to_a_header_and_header_is_not_present_it_should_be_added() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.AppendToHeader("Test", "Test");

            httpContext.Response.Headers.Keys.Should().Contain("Test");
            httpContext.Response.Headers["Test"].Equals("Test").Should().BeTrue();
        }

        [Fact]
        public void When_appending_to_a_header_it_should_be_appended() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Abcd" });

            internalContext.AppendToHeader("Test", "Test");

            httpContext.Response.Headers["Test"].ToString().Should().Be("Abcd,Test");
        }

        [Fact]
        public void When_checking_if_an_absent_header_exist_it_should_return_false() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.HeaderExist("Test").Should().BeFalse();
        }

        [Fact]
        public void When_checking_if_a_present_header_exist_it_should_return_true() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Test" });

            internalContext.HeaderExist("Test").Should().BeTrue();
        }

        [Fact]
        public void When_overriding_an_absent_header_it_should_be_added() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);

            internalContext.OverrideHeader("Test", "Test");

            httpContext.Response.Headers.Keys.Should().Contain("Test");
            httpContext.Response.Headers["Test"].Equals("Test").Should().BeTrue();
        }

        [Fact]
        public void When_overriding_a_header_the_value_should_be_overridden() {
            var httpContext = new DefaultHttpContext();
            var internalContext = new AspNetCoreHttpContext(httpContext);
            httpContext.Response.Headers.Add("Test", new[] { "Test" });

            internalContext.OverrideHeader("Test", "krznbf");

            httpContext.Response.Headers["Test"].Equals("krznbf").Should().BeTrue();
        }
    }
}