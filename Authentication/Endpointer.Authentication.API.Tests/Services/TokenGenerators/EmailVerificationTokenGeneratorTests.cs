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

        private string _email;
        private DateTime _expirationTime;

        [SetUp]
        public void Setup()
        {
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _configuration = new EmailVerificationConfiguration()
            {
                EmailVerificationTokenSecret = "12345",
                Audience = "localhost:5000/audience",
                Issuer = "localhost:5000/issuer",
                EmailVerificationTokenExpirationTime = DateTime.Now.AddDays(3)
            };

            _tokenGenerator = new EmailVerificationTokenGenerator(
                _configuration, 
                _mockTokenGenerator.Object,
                new Mock<ILogger<EmailVerificationTokenGenerator>>().Object);

            _email = "test@gmail.com";
        }

        [Test]
        public void GenerateToken_WithFailure_ThrowsException()
        {
            _mockTokenGenerator.Setup(ExpectedMockTokenGeneratorCallSetup()).Throws(new Exception());

            Assert.Throws<Exception>(() => _tokenGenerator.GenerateToken(_email));
        }

        [Test]
        public void GenerateToken_WithSuccess_ReturnsToken()
        {
            string expected = "123token123";
            _mockTokenGenerator.Setup(ExpectedMockTokenGeneratorCallSetup()).Returns(expected);

            string actual = _tokenGenerator.GenerateToken(_email);

            Assert.AreEqual(expected, actual);
        }

        private Expression<Func<ITokenGenerator, string>> ExpectedMockTokenGeneratorCallSetup()
        {
            return s => s.GenerateToken(
                _configuration.EmailVerificationTokenSecret, 
                _configuration.Issuer, 
                _configuration.Audience, 
                _configuration.EmailVerificationTokenExpirationTime, 
                It.IsAny<IEnumerable<Claim>>());
        }
    }
}
