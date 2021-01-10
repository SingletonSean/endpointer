using NUnit.Framework;
using Endpointer.Accounts.API.EndpointHandlers;
using System;
using Moq;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Core.API.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Endpointer.Core.API.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;

namespace Endpointer.Accounts.API.Tests.EndpointHandlers
{
    [TestFixture()]
    public class GetAccountEndpointHandlerTests
    {
        private GetAccountEndpointHandler _handler;

        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IHttpRequestAuthenticator> _mockHttpRequestAuthenticator;

        private HttpRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockHttpRequestAuthenticator = new Mock<IHttpRequestAuthenticator>();

            _handler = new GetAccountEndpointHandler(_mockAccountRepository.Object, 
                _mockHttpRequestAuthenticator.Object,
                new Mock<ILogger<GetAccountEndpointHandler>>().Object);

            _request = new Mock<HttpRequest>().Object;
        }

        [Test]
        public async Task HandleGetAccount_WithBearerSchemeNotProvidedException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new BearerSchemeNotProvidedException());

            IActionResult result = await _handler.HandleGetAccount(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public async Task HandleGetAccount_WithSecurityTokenDecryptionFailedException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new SecurityTokenDecryptionFailedException());

            IActionResult result = await _handler.HandleGetAccount(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public async Task HandleGetAccount_WithSecurityTokenException_ReturnsUnauthorizedResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ThrowsAsync(new SecurityTokenException());

            IActionResult result = await _handler.HandleGetAccount(_request);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public void HandleGetAccount_WithException_ThrowsException()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ReturnsAsync(new User());
            _mockAccountRepository.Setup(s => s.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleGetAccount(_request));
        }

        [Test]
        public async Task HandleGetAccount_WithAccountNotFound_ReturnsNotFoundResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ReturnsAsync(new User());
            _mockAccountRepository.Setup(s => s.GetById(It.IsAny<Guid>())).ReturnsAsync(() => null);

            IActionResult result = await _handler.HandleGetAccount(_request);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public async Task HandleGetAccount_WithAccountFound_ReturnsOkResult()
        {
            _mockHttpRequestAuthenticator.Setup(s => s.Authenticate(_request)).ReturnsAsync(new User());
            _mockAccountRepository.Setup(s => s.GetById(It.IsAny<Guid>())).ReturnsAsync(new User());

            IActionResult result = await _handler.HandleGetAccount(_request);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }
    }
}