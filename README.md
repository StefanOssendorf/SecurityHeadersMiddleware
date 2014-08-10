Security Headers Middleware
===========

[![Build status](https://ci.appveyor.com/api/projects/status/6n9xkyyvox9uw2up)](https://ci.appveyor.com/project/StefanOssendorf/securityheadersmiddleware) [![NuGet Status](http://img.shields.io/nuget/v/SecurityHeadersMiddleware.svg?style=flat)](https://www.nuget.org/packages/SecurityHeadersMiddleware/)

Middlewares to set useful security-related HTTP headers in your OWIN application. (From [OWASP list](https://www.owasp.org/index.php/List_of_useful_HTTP_headers "OWASP list"))

**Already implemented**
- Strict-Transport-Security incl. options
- X-Frame-Options incl. supporting multiple origins
- X-XSS-Protection inlc. disabling (but I don't know why).
- X-Content-Type-Options
- Content-Security-Policy

**Outstanding**

- Content-Security-Policy-Report-Only

#### Using
See the tests as examples of usage:
- [Strict-Transport-Security](https://github.com/StefanOssendorf/OwinContrib.SecurityHeaders/blob/master/src/OwinContrib.SecurityHeaders.Tests/StrictTransportSecurityMiddlewareSpecs.cs)
- [X-Frame-Options](https://github.com/StefanOssendorf/OwinContrib.SecurityHeaders/blob/master/src/OwinContrib.SecurityHeaders.Tests/AntiClickJackingMiddlewareSpecs.cs)
- [X-XSS-Protection](https://github.com/StefanOssendorf/OwinContrib.SecurityHeaders/blob/master/src/OwinContrib.SecurityHeaders.Tests/XssProtectionHeaderMiddlewareSpecs.cs)
- [X-Content-Type-Options](https://github.com/StefanOssendorf/OwinContrib.SecurityHeaders/blob/master/src/OwinContrib.SecurityHeaders.Tests/ContentTypeOptionsMiddleware.cs)
- [Content-Security-Policy](https://github.com/StefanOssendorf/SecurityHeadersMiddleware/blob/master/src/OwinContrib.SecurityHeaders.Tests/CspMiddlewareTests.cs) <br/> For Source-List usage see [Content-Security-Policy source lists](https://github.com/StefanOssendorf/SecurityHeadersMiddleware/blob/master/src/OwinContrib.SecurityHeaders.Tests/CspSourceListTests.cs) 

#### Developed with
[MarkdownPad 2](http://markdownpad.com/ "MarkdownPad 2")
[JetBrains ReSharper](http://www.jetbrains.com/resharper/ "R#")
