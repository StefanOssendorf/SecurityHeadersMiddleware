using System;
using Machine.Specifications;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class HostSourceTests {
        [Fact]
        public void When_creating_a_valid_full_host_source_it_should_not_throw_a_formatException() {
            new HostSource("http://*.example.com:80/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_without_schemePart_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com:80/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_only_path_part_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_only_host_part_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com");
        }

        [Fact]
        public void When_creating_an_invalid_host_part_it_should_throw_a_formatException() {
            Assert.Throws<FormatException>(() => new HostSource("ftp://*.example./abcd/"));
        }

        [Fact]
        public void When_creating_an_invalid_port_part_it_should_throw_a_formatException() {
            Assert.Throws<FormatException>(() => new HostSource("*.example.com:1as"));
        }

        [Fact]
        public void When_creating_an_invalid_path_it_should_throw_a_formatException() {
            Assert.Throws<FormatException>(() => new HostSource("*.example.com/%1"));
        }

        [Fact]
        public void When_creating_an_empty_host_it_should_throw_a_argumentException() {
            Assert.Throws<ArgumentException>(() => new HostSource(""));
        }
         
    }
}