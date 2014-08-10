using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class ReferrerKeywordTests {
        [Fact]
        public void When_set_Referrer_to_NotSet_it_should_not_create_any_headerValue() {
            GetBuilder(ReferrerKeyword.NotSet).ToDirectiveValue().Should().BeEmpty();
        }

        [Fact]
        public void ReferrerKeywords_should_create_the_correct_headerValues() {
            GetBuilder(ReferrerKeyword.NoneWhenDowngrade).ToDirectiveValue().Should().Be("none-when-downgrade");
            GetBuilder(ReferrerKeyword.None).ToDirectiveValue().Should().Be("none");
            GetBuilder(ReferrerKeyword.Origin).ToDirectiveValue().Should().Be("origin");
            GetBuilder(ReferrerKeyword.OriginWhenCossOrigin).ToDirectiveValue().Should().Be("origin-when-cross-origin");
            GetBuilder(ReferrerKeyword.UnsafeUrl).ToDirectiveValue().Should().Be("unsafe-url");

        }

        private static IDirectiveValueBuilder GetBuilder(ReferrerKeyword word) {
            return ReferrerDirectiveValueBuilder.Get(word);
        }
    }
}