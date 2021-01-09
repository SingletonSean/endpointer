using AutoMapper;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture]
    class RefreshEndpointHandlerTests
    {
        private RefreshEndpointHandler _handler;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private Mock<IAuthenticator> _mockAuthenticator;
        private Mock<IRefreshTokenValidator> _mockRefreshTokenValidator;
        private Mock<IMapper> _mockMapper;

        private RefreshRequest _request;
        private ModelStateDictionary _validModelState;
        private ModelStateDictionary _invalidModelState;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _mockAuthenticator = new Mock<IAuthenticator>();
            _mockRefreshTokenValidator = new Mock<IRefreshTokenValidator>();
            _mockMapper = new Mock<IMapper>();

            _handler = new RefreshEndpointHandler(
                _mockUserRepository.Object,
                _mockRefreshTokenRepository.Object,
                _mockAuthenticator.Object,
                _mockRefreshTokenValidator.Object,
                _mockMapper.Object);

            _request = new RefreshRequest();
            _validModelState = new ModelStateDictionary();
            _invalidModelState = new ModelStateDictionary();
            _invalidModelState.AddModelError("Error", "Message");
        }

        [Test]
        public async Task HandleRefresh_WithInvalidModelState_ReturnsBadRequestResult()
        {
            IActionResult result = await _handler.HandleRefresh(_request, _invalidModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task HandleRefresh_WithInvalidToken_ReturnsBadRequestResult()
        {
            _mockRefreshTokenValidator.Setup(v => v.Validate(_request.RefreshToken)).Returns(false);

            IActionResult result = await _handler.HandleRefresh(_request, _validModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task HandleRefresh_WithTokenNotFound_ReturnsNotFoundResult()
        {
            _mockRefreshTokenValidator.Setup(v => v.Validate(_request.RefreshToken)).Returns(true);
            _mockRefreshTokenRepository.Setup(s => s.GetByToken(_request.RefreshToken)).ReturnsAsync(() => null);

            IActionResult result = await _handler.HandleRefresh(_request, _validModelState);

            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Test]
        public void HandleRefresh_WithException_ThrowsException()
        {
            _mockRefreshTokenValidator.Setup(v => v.Validate(_request.RefreshToken)).Returns(true);
            _mockRefreshTokenRepository.Setup(s => s.GetByToken(_request.RefreshToken)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleRefresh(_request, _validModelState));
        }

        [Test]
        public async Task HandleRefresh_WithRefreshTokenUserNotFound_ReturnsNotFoundResult()
        {
            RefreshToken refreshToken = new RefreshToken() { UserId = Guid.NewGuid() };
            _mockRefreshTokenValidator.Setup(v => v.Validate(_request.RefreshToken)).Returns(true);
            _mockRefreshTokenRepository.Setup(s => s.GetByToken(_request.RefreshToken)).ReturnsAsync(refreshToken);
            _mockUserRepository.Setup(s => s.GetById(refreshToken.UserId)).ReturnsAsync(() => null);

            IActionResult result = await _handler.HandleRefresh(_request, _validModelState);

            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public async Task HandleRefresh_WithSuccess_ReturnsOkResult()
        {
            RefreshToken refreshToken = new RefreshToken() { UserId = Guid.NewGuid() };
            _mockRefreshTokenValidator.Setup(v => v.Validate(_request.RefreshToken)).Returns(true);
            _mockRefreshTokenRepository.Setup(s => s.GetByToken(_request.RefreshToken)).ReturnsAsync(refreshToken);
            _mockUserRepository.Setup(s => s.GetById(refreshToken.UserId)).ReturnsAsync(new User());

            IActionResult result = await _handler.HandleRefresh(_request, _validModelState);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }
    }
}
