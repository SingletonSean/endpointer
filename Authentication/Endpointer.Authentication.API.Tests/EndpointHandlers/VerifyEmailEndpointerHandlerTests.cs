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

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture()]
    public class VerifyEmailEndpointerHandlerTests : EndpointHandlerTests
    {
        private VerifyEmailEndpointerHandler _handler;

        private Mock<IEmailVerificationTokenValidator> _tokenValidator;

        private VerifyEmailRequest _request;

        [SetUp]
        public void SetUp()
        {
            _tokenValidator = new Mock<IEmailVerificationTokenValidator>();

            _handler = new VerifyEmailEndpointerHandler(_tokenValidator.Object, new Mock<ILogger<VerifyEmailEndpointerHandler>>().Object);

            _request = new VerifyEmailRequest()
            {
                VerifyToken = "123token123"
            };
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
            _tokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Throws(new SecurityTokenException());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithInvalidTokenSecurityTokenDecryptionFailedException_ReturnsUnauthorizedResult()
        {
            _tokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Throws(new SecurityTokenDecryptionFailedException());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithValidToken_ReturnsOkResult()
        {
            _tokenValidator.Setup(v => v.Validate(_request.VerifyToken)).Returns(new EmailVerificationToken());

            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Test()]
        public async Task HandleVerifyEmail_WithValidToken_SetsUserAsEmailVerified()
        {
            IActionResult result = await _handler.HandleVerifyEmail(_request, _validModelState);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }
    }
}