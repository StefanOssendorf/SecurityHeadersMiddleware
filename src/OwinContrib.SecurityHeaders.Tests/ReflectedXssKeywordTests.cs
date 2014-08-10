using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class ReflectedXssKeywordTests {
        [Fact]
        public void When_set_reflectedXss_to_NotSet_it_should_not_create_any_headerValue() {
            GetBuilder(ReflectedXssKeyword.NotSet).ToDirectiveValue().Should().BeEmpty();
        }

        [Fact]
        public void ReflectedXssKeywords_should_create_the_correct_headerValues() {
            GetBuilder(ReflectedXssKeyword.Allow).ToDirectiveValue().Should().Be("allow");
            GetBuilder(ReflectedXssKeyword.Block).ToDirectiveValue().Should().Be("block");
            GetBuilder(ReflectedXssKeyword.Filter).ToDirectiveValue().Should().Be("filter");
        }

        private static IDirectiveValueBuilder GetBuilder(ReflectedXssKeyword word) {
            return ReflectedXssDirectiveValueBuilder.Get(word);
        }
    }
}