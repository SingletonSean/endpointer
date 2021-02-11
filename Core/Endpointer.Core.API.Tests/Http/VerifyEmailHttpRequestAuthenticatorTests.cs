using NUnit.Framework;
using Endpointer.Core.API.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.AspNetCore.Http;
using Endpointer.Core.API.Models;
using Endpointer.Core.API.Exceptions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Endpointer.Core.API.Tests.Http
{
    [TestFixture()]
    public class VerifyEmailHttpRequestAuthenticatorTests
    {
        private VerifyEmailHttpRequestAuthenticator _authenticator;

        private Mock<IHttpRequestAuthenticator> _baseAuthenticator;

        private HttpRequest _httpRequest;

        [SetUp]
        public void SetUp()
        {
            _baseAuthenticator = new Mock<IHttpRequestAuthenticator>();

            _authenticator = new VerifyEmailHttpRequestAuthenticator(_baseAuthenticator.Object, new Mock<ILogger<VerifyEmailHttpRequestAuthenticator>>().Object);

            _httpRequest = new Mock<HttpRequest>().Object;
        }

        [Test()]
        public void Authenticate_WithBaseAuthenticatorThrowing_ThrowsException()
        {
            _baseAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _authenticator.Authenticate(_httpRequest));
        }

        [Test()]
        public void Authenticate_WithBaseAuthenticatorReturningUnverifiedEmailUser_ThrowsUnverifiedEmailExceptionForEmail()
        {
            string email = "test@gmail.com";
            _baseAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ReturnsAsync(new User() { Email = email, IsEmailVerified = false });

            UnverifiedEmailException exception = Assert.ThrowsAsync<UnverifiedEmailException>(() => _authenticator.Authenticate(_httpRequest));

            Assert.AreEqual(email, exception.Email);
        }

        [Test()]
        public async Task Authenticate_WithBaseAuthenticatorReturningVerifiedEmailUser_ReturnsUserWithEmailVerified()
        {
            string email = "test@gmail.com";
            _baseAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ReturnsAsync(new User() { Email = email, IsEmailVerified = true });

            User user = await _authenticator.Authenticate(_httpRequest);

            Assert.IsTrue(user.IsEmailVerified);
            Assert.AreEqual(email, user.Email);
        }
    }
}