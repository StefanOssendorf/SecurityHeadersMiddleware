using Machine.Specifications;
using SecurityHeadersMiddleware.Infrastructure;
using Xunit;

namespace SecurityHeadersMiddleware.Tests.Infrastructure {
    public class Rfc7230UtilityTests {
        [Fact]
        public void When_validate_null_as_token_it_should_return_false() {
            Rfc7230Utility.IsToken(null).ShouldBeFalse();
        } 
    }
}