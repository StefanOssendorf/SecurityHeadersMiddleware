using System;
using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspSandboxTokenListTests {
        [Fact]
        public void When_adding_an_invalid_sandboxToken_it_should_throw_a_formatException() {
            var list = new CspSandboxTokenList();
            Assert.Throws<FormatException>(() => list.AddToken("acd\"2asda"));
        }

        [Fact]
        public void When_adding_a_valid_sandboxToken_it_should_create_a_headerValue() {
            var list = new CspSandboxTokenList();
            list.AddToken("allow-scripts");

            list.ToDirectiveValue().Should().Be("allow-scripts");
        }

        [Fact]
        public void When_adding_a_token_twice_it_should_only_be_once_in_the_headerValue() {
            var list = new CspSandboxTokenList();
            list.AddToken("allow-scripts");
            list.AddToken("allow-scripts");

            list.ToDirectiveValue().Trim().Should().Be("allow-scripts");
        }
    }
}