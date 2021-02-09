using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Models.Users;
using Endpointer.Authentication.API.Services.PasswordHashers;
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
    class RegisterEndpointHandlerTests
    {
        private RegisterEndpointHandler _handler;

        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IUserFactory> _mockUserFactory;

        private RegisterRequest _request;
        private ModelStateDictionary _validModelState;
        private ModelStateDictionary _invalidModelState;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockUserFactory = new Mock<IUserFactory>();

            _handler = new RegisterEndpointHandler(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockUserFactory.Object,
                new Mock<ILogger<RegisterEndpointHandler>>().Object);

            _request = new RegisterRequest()
            {
                Password = "match",
                ConfirmPassword = "match"
            };
            _validModelState = new ModelStateDictionary();
            _invalidModelState = new ModelStateDictionary();
            _invalidModelState.AddModelError("Error", "Message");
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
            _mockUserRepository.Setup(s => s.Create(It.IsAny<User>())).ThrowsAsync(new Exception());

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
        public async Task HandleRegister_WithSuccess_CreatesUser()
        {
            _mockUserRepository.Setup(s => s.GetByEmail(_request.Email)).ReturnsAsync(() => null);
            _mockUserRepository.Setup(s => s.GetByUsername(_request.Username)).ReturnsAsync(() => null);

            await _handler.HandleRegister(_request, _validModelState);

            _mockUserFactory.Verify(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockUserRepository.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        }
    }
}
