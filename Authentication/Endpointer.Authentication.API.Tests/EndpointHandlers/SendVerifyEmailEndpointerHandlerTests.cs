using NUnit.Framework;
using Endpointer.Authentication.API.EndpointHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Endpointer.API.Tests.EndpointHandlers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using Endpointer.Core.API.Http;
using Endpointer.Authentication.API.Services.EmailVerificationSenders;
using Endpointer.Core.API.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Endpointer.Core.API.Models;

namespace Endpointer.Authentication.API.Tests.EndpointHandlers
{
    [TestFixture()]
    public class SendVerifyEmailEndpointerHandlerTests : EndpointHandlerTests
    {
        private SendVerifyEmailEndpointerHandler _handler;

        private Mock<IHttpRequestAuthenticator> _mockAuthenticator;
        private Mock<IEmailVerificationSender> _mockEmailSender;

        [SetUp]
        public void SetUp()
        {
            _mockAuthenticator = new Mock<IHttpRequestAuthenticator>();
            _mockEmailSender = new Mock<IEmailVerificationSender>();

            _handler = new SendVerifyEmailEndpointerHandler(
                _mockAuthenticator.Object,
                _mockEmailSender.Object,
                new Mock<ILogger<SendVerifyEmailEndpointerHandler>>().Object);
        }

        [Test()]
        public async Task HandleSendVerifyEmail_WithBearerSchemeException_ReturnsUnauthorizedResult()
        {
            _mockAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ThrowsAsync(new BearerSchemeNotProvidedException());

            IActionResult result = await _handler.HandleSendVerifyEmail(_httpRequest);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleSendVerifyEmail_WithSecurityTokenException_ReturnsUnauthorizedResult()
        {
            _mockAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ThrowsAsync(new SecurityTokenException());

            IActionResult result = await _handler.HandleSendVerifyEmail(_httpRequest);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public async Task HandleSendVerifyEmail_WithSecurityTokenDecryptionFailedException_ReturnsUnauthorizedResult()
        {
            _mockAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ThrowsAsync(new SecurityTokenDecryptionFailedException());

            IActionResult result = await _handler.HandleSendVerifyEmail(_httpRequest);

            Assert.IsAssignableFrom<UnauthorizedResult>(result);
        }

        [Test()]
        public void HandleSendVerifyEmail_WithEmailSendException_ThrowsException()
        {
            _mockEmailSender.Setup(s => s.SendEmailVerificationEmail(It.IsAny<User>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _handler.HandleSendVerifyEmail(_httpRequest));
        }

        [Test()]
        public async Task HandleSendVerifyEmail_WithSuccess_ReturnsOkResult()
        {
            IActionResult result = await _handler.HandleSendVerifyEmail(_httpRequest);

            Assert.IsAssignableFrom<OkResult>(result);
        }

        [Test()]
        public async Task HandleSendVerifyEmail_WithSuccess_SendsEmailToUser()
        {
            User user = new User();
            _mockAuthenticator.Setup(s => s.Authenticate(_httpRequest)).ReturnsAsync(user);

            await _handler.HandleSendVerifyEmail(_httpRequest);

            _mockEmailSender.Verify(s => s.SendEmailVerificationEmail(user), Times.Once);
        }
    }
}