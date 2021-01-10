using System.Threading.Tasks;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace Endpointer.Core.API.Tests.Http
{
    [TestFixture]
    public class HttpRequestAuthenticatorTests
    {
        private HttpRequestAuthenticator _authenticator;

        private Mock<IAccessTokenDecoder> _mockAccessTokenDecoder;

        private HttpRequest _httpRequest;
        private Mock<IHeaderDictionary> _mockHttpRequestHeaders;

        [SetUp]
        public void SetUp()
        {
            _mockAccessTokenDecoder = new Mock<IAccessTokenDecoder>();

            _authenticator = new HttpRequestAuthenticator(_mockAccessTokenDecoder.Object, new Mock<ILogger<HttpRequestAuthenticator>>().Object);

            _mockHttpRequestHeaders = new Mock<IHeaderDictionary>();
            Mock<HttpRequest> mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(r => r.Headers).Returns(_mockHttpRequestHeaders.Object);
            _httpRequest = mockHttpRequest.Object;
        }

        [Test]
        public void Authenticate_WithNullAuthorizationHeader_ThrowsBearerSchemeNotProvidedException()
        {
            Assert.ThrowsAsync<BearerSchemeNotProvidedException>(() => _authenticator.Authenticate(_httpRequest));
        }

        [Test]
        public void Authenticate_WithEmptyAuthorizationHeader_ThrowsBearerSchemeNotProvidedException()
        {
            _mockHttpRequestHeaders.SetupGet(r => r["Authorization"]).Returns(string.Empty);

            Assert.ThrowsAsync<BearerSchemeNotProvidedException>(() => _authenticator.Authenticate(_httpRequest));
        }

        [Test]
        public void Authenticate_WithMissingBearerScheme_ThrowsBearerSchemeNotProvidedException()
        {
            _mockHttpRequestHeaders.SetupGet(r => r["Authorization"]).Returns("123sometoken123");

            Assert.ThrowsAsync<BearerSchemeNotProvidedException>(() => _authenticator.Authenticate(_httpRequest));
        }

        [Test]
        public void Authenticate_WithSecurityTokenFailure_ThrowsSecurityTokenException()
        {
            SetupValidBearerToken();
            _mockAccessTokenDecoder.Setup(d => d.GetUserFromToken(It.IsAny<string>())).ThrowsAsync(new SecurityTokenException());

            Assert.ThrowsAsync<SecurityTokenException>(() => _authenticator.Authenticate(_httpRequest));
        }

        [Test]
        public void Authenticate_WithSecurityTokenDecryptionFailure_ThrowsSecurityTokenDecryptionFailedException()
        {
            SetupValidBearerToken();
            _mockAccessTokenDecoder.Setup(d => d.GetUserFromToken(It.IsAny<string>())).ThrowsAsync(new SecurityTokenDecryptionFailedException());

            Assert.ThrowsAsync<SecurityTokenDecryptionFailedException>(() => _authenticator.Authenticate(_httpRequest));

        }

        [Test]
        public async Task Authenticate_WithValidToken_ReturnsUser()
        {
            SetupValidBearerToken();
            _mockAccessTokenDecoder.Setup(d => d.GetUserFromToken(It.IsAny<string>())).ReturnsAsync(new User());

            User user = await _authenticator.Authenticate(_httpRequest);

            Assert.IsNotNull(user);
        }

        private void SetupValidBearerToken()
        {
            _mockHttpRequestHeaders.SetupGet(r => r["Authorization"]).Returns("Bearer 123sometoken123");
        }
    }
}