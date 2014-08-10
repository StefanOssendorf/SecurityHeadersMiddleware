using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspUriReferenceListTests {
        [Fact]
        public void When_adding_an_invalid_uri_it_should_throw_a_formatException() {
            var refList = new CspUriReferenceList();

            Assert.Throws<FormatException>(() => refList.AddReportUri("http//example.org"));
        }

        [Fact]
        public void When_adding_a_valid_uri_it_should_create_the_headerValue() {
            var list = new CspUriReferenceList();
            list.AddReportUri("http://www.example.com");

            list.ToDirectiveValue().Trim().Should().Be("http://www.example.com");
        }

        [Fact]
        public void When_adding_a_uri_twice_it_should_only_be_once_in_the_headerValue() {
            var list = new CspUriReferenceList();
            list.AddReportUri("http://www.example.com");
            list.AddReportUri("http://www.example.com");

            list.ToDirectiveValue().Trim().Should().Be("http://www.example.com");
        }

        [Fact]
        public void When_adding_several_uris_they_should_be_separated_by_at_least_one_whitespace() {
            var compareList = new List<string> {
                "http://www.example.com".ToLower(),
                "http://www.example.com/list?id=10#Fragment=12".ToLower(),
                "http://www.example.com/list?id=10".ToLower(),
                "http://www.example.com/list".ToLower()
            };
            var list = new CspUriReferenceList();
            list.AddReportUri("http://www.example.com");
            list.AddReportUri("http://www.example.com/list");
            list.AddReportUri("http://www.example.com/list?id=10");
            list.AddReportUri("http://www.example.com/list?id=10#Fragment=12");

            var split = list.ToDirectiveValue().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(val => val.Trim()).ToArray();
            split.Length.Should().Be(4);
            split.Should().Contain(compareList);
        }
    }
}