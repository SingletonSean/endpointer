using NUnit.Framework;
using Endpointer.Authentication.Client.Services.SendVerifyEmail;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Client.Tests.Services;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Refit;
using Endpointer.Core.Client.Exceptions;
using System.Net;

namespace Endpointer.Authentication.Client.Tests.Services.SendVerifyEmail
{
    [TestFixture()]
    public class SendVerifyEmailServiceTests : APIServiceTests
    {
        private SendVerifyEmailService _service;

        private Mock<IAPISendVerifyEmailService> _mockAPI;

        [SetUp]
        public void SetUp()
        {
            _mockAPI = new Mock<IAPISendVerifyEmailService>();

            _service = new SendVerifyEmailService(_mockAPI.Object, new Mock<ILogger<SendVerifyEmailService>>().Object);
        }

        [Test()]
        public async Task SendVerifyEmail_WithSuccess_CallsAPI()
        {
            await _service.SendVerifyEmail();

            _mockAPI.Verify(s => s.SendVerifyEmail(), Times.Once);
        }

        [Test()]
        public void SendVerifyEmail_WithSuccess_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(() => _service.SendVerifyEmail());
        }

        [Test()]
        public void SendVerifyEmail_WithUnknownException_ThrowsException()
        {
            _mockAPI.Setup(s => s.SendVerifyEmail()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _service.SendVerifyEmail());
        }

        [Test()]
        public async Task SendVerifyEmail_WithUnauthorizedAPIException_ThrowsUnauthorizedException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.Unauthorized);
            _mockAPI.Setup(s => s.SendVerifyEmail()).ThrowsAsync(exception);

            Assert.ThrowsAsync<UnauthorizedException>(() => _service.SendVerifyEmail());
        }

        [Test()]
        public async Task SendVerifyEmail_WithUnknownAPIException_ThrowsAPIException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.InternalServerError);
            _mockAPI.Setup(s => s.SendVerifyEmail()).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.SendVerifyEmail());
        }
    }
}