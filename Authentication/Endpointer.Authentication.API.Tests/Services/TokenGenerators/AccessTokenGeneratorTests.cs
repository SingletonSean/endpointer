using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Core.API.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Tests.Services.TokenGenerators
{
    [TestFixture]
    public class AccessTokenGeneratorTests
    {
        private AccessTokenGenerator _tokenGenerator;

        private Mock<ITokenGenerator> _mockTokenGenerator;

        private User _user;

        [SetUp]
        public void Setup()
        {
            _mockTokenGenerator = new Mock<ITokenGenerator>();

            _tokenGenerator = new AccessTokenGenerator(new AuthenticationConfiguration(), _mockTokenGenerator.Object);

            _user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                Username = "test"
            };
        }

        [Test]
        public void GenerateToken_WithFailure_ThrowsException()
        {
            _mockTokenGenerator.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<IEnumerable<Claim>>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _tokenGenerator.GenerateToken(_user, DateTime.Now.AddDays(3)));
        }

        [Test]
        public void GenerateToken_WithSuccess_ReturnsToken()
        {
            string expected = "123token123";
            _mockTokenGenerator.Setup(s => s.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<IEnumerable<Claim>>())).Returns(expected);

            string actual = _tokenGenerator.GenerateToken(_user, DateTime.Now.AddDays(3));

            Assert.AreEqual(expected, actual);
        }
    }
}
