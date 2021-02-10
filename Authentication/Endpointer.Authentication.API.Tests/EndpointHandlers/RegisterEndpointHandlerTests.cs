using Endpointer.API.Tests.EndpointHandlers;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.UserRegisters;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    public class RegisterEndpointHandlerTests : EndpointHandlerTests
    {
        private RegisterEndpointHandler _handler;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IUserRegister> _mockUserRegister;

        private RegisterRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUserRegister = new Mock<IUserRegister>();

            _handler = new RegisterEndpointHandler(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockUserRegister.Object,
                new Mock<ILogger<RegisterEndpointHandler>>().Object);

            _request = new RegisterRequest()
            {
                Password = "match",
                ConfirmPassword = "match"
            };
        }

        [Test]
        public async Task HandleRegister_WithInvalidModelState_ReturnsBadRequestResult()
        {
            IActionResult result = await _handler.HandleRegister(_request, _invalidModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task HandleRegister_WithPasswordsNotMatching_ReturnsBadRequestResult()
        {
            _request.ConfirmPassword = "not matching";

            IActionResult result = await _handler.HandleRegister(_request, _validModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task HandleRegister_WithExistingEmail_ReturnsConflictResult()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(new User());

            IActionResult result = await _handler.HandleRegister(_request, _validModelState);

            Assert.IsAssignableFrom<ConflictObjectResult>(result);
        }

        [Test]
        public async Task HandleRegister_WithExistingUsername_ReturnsConflictResult()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(() => null);
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(new User());

            IActionResult result = await _handler.HandleRegister(_request, _validModelState);

            Assert.IsAssignableFrom<ConflictObjectResult>(result);
        }

        [Test]
        public void HandleRegister_WithException_ThrowsException()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(() => null);
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(() => null);
            _mockUserRegister.Setup(s => s.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleRegister(_request, _validModelState));
        }

        [Test]
        public async Task HandleRegister_WithSuccess_ReturnsOkResult()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(() => null);
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(() => null);

            IActionResult result = await _handler.HandleRegister(_request, _validModelState);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Test]
        public async Task HandleRegister_WithSuccess_RegistersUser()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(() => null);
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(() => null);

            await _handler.HandleRegister(_request, _validModelState);

            _mockUserRegister.Verify(f => f.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
