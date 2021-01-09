using AutoMapper;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture]
    public class LoginEndpointHandlerTests
    {
        private LoginEndpointHandler _handler;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IAuthenticator> _mockAuthenticator;
        private Mock<IMapper> _mockMapper;

        private LoginRequest _request;
        private ModelStateDictionary _validModelState;
        private ModelStateDictionary _invalidModelState;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockAuthenticator = new Mock<IAuthenticator>();
            _mockMapper = new Mock<IMapper>();

            _handler = new LoginEndpointHandler(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockAuthenticator.Object,
                _mockMapper.Object);

            _request = new LoginRequest();
            _validModelState = new ModelStateDictionary();
            _invalidModelState = new ModelStateDictionary();
            _invalidModelState.AddModelError("Error", "Message");
        }

        [Test]
        public async Task HandleLogin_WithInvalidModelState_ReturnsBadRequestResult()
        {
            IActionResult result = await _handler.HandleLogin(_request, _invalidModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task HandleLogin_WithUsernameNotFound_ReturnsUnauthorizedResult()
        {
            IActionResult result = await _handler.HandleLogin(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public async Task HandleLogin_WithIncorrectPassword_ReturnsUnauthorizedResult()
        {
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(new User());
            _mockPasswordHasher.Setup(s => s.VerifyPassword(_request.Password, It.IsAny<string>())).Returns(false);

            IActionResult result = await _handler.HandleLogin(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test]
        public void HandleLogin_WithException_ThrowsException()
        {
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleLogin(_request, _validModelState));
        }

        [Test]
        public async Task HandleLogin_WithCorrectCredentials_ReturnsOkResult()
        {
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(new User());
            _mockPasswordHasher.Setup(s => s.VerifyPassword(_request.Password, It.IsAny<string>())).Returns(true);

            IActionResult result = await _handler.HandleLogin(_request, _validModelState);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }
    }
}
