using NUnit.Framework;
using Endpointer.Authentication.API.Services.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Microsoft.Extensions.Logging;
using Endpointer.Core.API.Models;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.Services.Authenticators
{
    [TestFixture]
    public class AuthenticatorTests
    {
        private Authenticator _authenticator;

        private Mock<IAccessTokenGenerator> _mockAccessTokenGenerator;
        private Mock<IRefreshTokenGenerator> _mockRefreshTokenGenerator;
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private AuthenticationConfiguration _configuration;
        private Mock<ILogger<Authenticator>> _mockLogger;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            _mockAccessTokenGenerator = new Mock<IAccessTokenGenerator>();
            _mockRefreshTokenGenerator = new Mock<IRefreshTokenGenerator>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _configuration = new AuthenticationConfiguration();
            _mockLogger = new Mock<ILogger<Authenticator>>();

            _authenticator = new Authenticator(
                _mockAccessTokenGenerator.Object,
                _mockRefreshTokenGenerator.Object,
                _mockRefreshTokenRepository.Object,
                _configuration,
                _mockLogger.Object);

            _user = new User();
        }

        [Test]
        public void Authenticate_WithException_ThrowsException()
        {
            _mockAccessTokenGenerator.Setup(s => s.GenerateToken(_user, It.IsAny<DateTime>())).Throws(new Exception());

            Assert.ThrowsAsync<Exception>(() => _authenticator.Authenticate(_user));
        }

        [Test]
        public async Task Authenticate_WithSuccess_ReturnsAuthenticatedUser()
        {
            string expectedAccessToken = "access_token";
            string expectedRefreshToken = "refresh_token";
            _mockAccessTokenGenerator.Setup(s => s.GenerateToken(_user, It.IsAny<DateTime>())).Returns(expectedAccessToken);
            _mockRefreshTokenGenerator.Setup(s => s.GenerateToken()).Returns(expectedRefreshToken);

            AuthenticatedUser user = await _authenticator.Authenticate(_user);

            Assert.AreEqual(expectedAccessToken, user.AccessToken);
            Assert.AreEqual(expectedRefreshToken, user.RefreshToken);
            Assert.AreNotEqual(new DateTime(), user.AccessTokenExpirationTime);
        }
    }
}
