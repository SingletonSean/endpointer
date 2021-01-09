using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture]
    public class LogoutEverywhereEndpointHandlerTests
    {
        private LogoutEverywhereEndpointHandler _handler;

        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private Mock<IHttpRequestAuthenticator> _mockHttpRequestAuthenticator;

        private HttpRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockHttpRequestAuthenticator = new Mock<IHttpRequestAuthenticator>();

            _handler = new LogoutEverywhereEndpointHandler(_mockRefreshTokenRepository.Object, _mockHttpRequestAuthenticator.Object);

            _request = new Mock<HttpRequest>().Object;
        }

        [Test]
        public async Task HandleLogoutEverywhere_WithBearerSchemeNotProvidedException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new BearerSchemeNotProvidedException());

            IActionResult result = await _handler.HandleLogoutEverywhere(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public async Task HandleLogoutEverywhere_WithSecurityTokenDecryptionFailedException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new SecurityTokenDecryptionFailedException());

            IActionResult result = await _handler.HandleLogoutEverywhere(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public async Task HandleLogoutEverywhere_WithSecurityTokenException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new SecurityTokenException());

            IActionResult result = await _handler.HandleLogoutEverywhere(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public void HandleLogoutEverywhere_WithException_ThrowsException()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ReturnsAsync(new User());
            _mockRefreshTokenRepository.Setup(s => s.DeleteAll(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleLogoutEverywhere(_request));
        }

        [Test]
        public async Task HandleLogoutEverywhere_WithSuccess_ReturnsNoContentResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ReturnsAsync(new User());

            IActionResult result = await _handler.HandleLogoutEverywhere(_request);

            Assert.IsAssignableFrom<NoContentResult>(result);
        }
    }
}
