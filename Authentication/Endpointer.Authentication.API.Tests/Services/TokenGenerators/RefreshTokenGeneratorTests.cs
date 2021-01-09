using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Endpointer.Authentication.API.Tests.Services.TokenGenerators
{
    [TestFixture]
    public class RefreshTokenGeneratorTests
    {
        private RefreshTokenGenerator _tokenGenerator;

        private Mock<ITokenGenerator> _mockTokenGenerator;

        [SetUp]
        public void Setup()
        {
            _mockTokenGenerator = new Mock<ITokenGenerator>();

            _tokenGenerator = new RefreshTokenGenerator(new AuthenticationConfiguration(), _mockTokenGenerator.Object);
        }

        [Test]
        public void GenerateToken_WithFailure_ThrowsException()
        {
            _mockTokenGenerator.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<IEnumerable<Claim>>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _tokenGenerator.GenerateToken());
        }

        [Test]
        public void GenerateToken_WithSuccess_ReturnsToken()
        {
            string expected = "123token123";
            _mockTokenGenerator.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<IEnumerable<Claim>>())).Returns(expected);

            string actual = _tokenGenerator.GenerateToken();

            Assert.AreEqual(expected, actual);
        }
    }
}
