using NUnit.Framework;
using Endpointer.Authentication.API.EndpointHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.API.Tests.EndpointHandlers;
using Endpointer.Authentication.Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Moq;
using Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications;
using Microsoft.IdentityModel.Tokens;
using Endpointer.Authentication.API.Models;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture()]
    public class VerifyEmailEndpointerHandlerTests : EndpointHandlerTests
    {
        private VerifyEmailEndpointerHandler _handler;

        private Mock<IEmailVerificationTokenValidator> _mockTokenValidator;
        private Mock<IUserRepository> _mockUserRepository;

        private VerifyEmailRequest _request;
        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _mockTokenValidator = new Mock<IEmailVerificationTokenValidator>();
            _mockUserRepository = new Mock<IUserRepository>();

            _handler = new VerifyEmailEndpointerHandler(
                _mockTokenValidator.Object, 
                _mockUserRepository.Object,
                new Mock<ILogger<VerifyEmailEndpointerHandler>>().Object);

            _request = new VerifyEmailRequest()
            {
                VerifyToken = "123token123"
            };
            _userId = Guid.NewGuid();
        }

        [Test()]
        public async Task HandleVerifyEmail_WithInvalidModelState_ReturnsBadRequestObjectResult()
        {
            IActionResult result = await _handler.HandleVerifyEmail(_request, _invalidModelState);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithInvalidTokenSecurityTokenException_ReturnsUnauthorizedResult()
        {
            _mockTokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Throws(new SecurityTokenException());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithInvalidTokenSecurityTokenDecryptionFailedException_ReturnsUnauthorizedResult()
        {
            _mockTokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Throws(new SecurityTokenDecryptionFailedException());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithValidToken_ReturnsOkResult()
        {
            _mockTokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Returns(new EmailVerificationToken());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithValidToken_SetsUserAsEmailVerified()
        {
            _mockTokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Returns(new EmailVerificationToken()
            {
                UserId = _userId
            });

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            _mockUserRepository.Verify(r => r.Update(_userId, It.IsAny<Action<User>>()), Times.Once);
        }

        [Test()]
        public void HandleVerifyEmail_WithUserUpdateFailure_ThrowsException()
        {
            _mockTokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Returns(new EmailVerificationToken()
            {
                UserId = _userId
            });
            _mockUserRepository.Setup(r => r.Update(_userId, It.IsAny<Action<User>>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleVerifyEmail(_request, _validModelState));
        }
    }
}