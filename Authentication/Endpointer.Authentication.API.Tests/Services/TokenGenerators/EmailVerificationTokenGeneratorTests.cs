using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Tests.Services.TokenGenerators.EmailVerifications
{
    [TestFixture]
    public class EmailVerificationTokenGeneratorTests
    {
        private EmailVerificationTokenGenerator _tokenGenerator;

        private Mock<ITokenGenerator> _mockTokenGenerator;
        private EmailVerificationConfiguration _configuration;

        private User _user;

        [SetUp]
        public void Setup()
        {
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _configuration = new EmailVerificationConfiguration()
            {
                TokenSecret = "12345",
                TokenAudience = "localhost:5000/audience",
                TokenIssuer = "localhost:5000/issuer",
                TokenExpirationMinutes = 5
            };

            _tokenGenerator = new EmailVerificationTokenGenerator(
                _configuration, 
                _mockTokenGenerator.Object,
                new Mock<ILogger<EmailVerificationTokenGenerator>>().Object);

            _user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com"
            };
        }

        [Test]
        public void GenerateToken_WithFailure_ThrowsException()
        {
            _mockTokenGenerator.Setup(ExpectedMockTokenGeneratorCallSetup()).Throws(new Exception());

            Assert.Throws<Exception>(() => _tokenGenerator.GenerateToken(_user));
        }

        [Test]
        public void GenerateToken_WithSuccess_ReturnsToken()
        {
            string expected = "123token123";
            _mockTokenGenerator.Setup(ExpectedMockTokenGeneratorCallSetup()).Returns(expected);

            string actual = _tokenGenerator.GenerateToken(_user);

            Assert.AreEqual(expected, actual);
        }

        private Expression<Func<ITokenGenerator, string>> ExpectedMockTokenGeneratorCallSetup()
        {
            return s => s.GenerateToken(
                _configuration.TokenSecret, 
                _configuration.TokenIssuer, 
                _configuration.TokenAudience, 
                It.IsAny<DateTime>(), 
                It.IsAny<IEnumerable<Claim>>());
        }
    }
}
