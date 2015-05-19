using System;
using System.Linq;
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

        [Fact]
        public void Keywords_should_create_the_correct_header_values() {
            var list = new CspSandboxTokenList();
            list.AddKeyword(SandboxKeyword.AllowForms);
            list.AddKeyword(SandboxKeyword.AllowPointerLock);
            list.AddKeyword(SandboxKeyword.AllowPopups);
            list.AddKeyword(SandboxKeyword.AllowSameOrigin);
            list.AddKeyword(SandboxKeyword.AllowScripts);
            list.AddKeyword(SandboxKeyword.AllowTopNavigation);
            var split = list.ToDirectiveValue().Split(new[] {" "}, StringSplitOptions.None).Select(item => item.Trim());
            var expectedValues = new[] {
                "allow-forms", "allow-pointer-lock", "allow-popups", "allow-same-origin", "allow-scripts", "allow-top-navigation"
            };
            split.Should().Contain(expectedValues);
        }

        [Fact]
        public void When_set_to_empty_it_should_return_an_empty_headerValue() {
            var list = new CspSandboxTokenList();
            list.SetToEmptyValue();
            list.ToDirectiveValue().Should().BeEmpty();
        }
    }
}