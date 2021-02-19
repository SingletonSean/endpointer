using NUnit.Framework;
using Endpointer.Authentication.Client.Services.VerifyEmail;
using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Client.Tests.Services;
using Moq;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using System.Net;
using Refit;
using System.Threading.Tasks;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;

namespace Endpointer.Authentication.Client.Tests.Services.VerifyEmail
{
    [TestFixture()]
    public class VerifyEmailServiceTests : APIServiceTests
    {
        private VerifyEmailService _service;

        private Mock<IAPIVerifyEmailService> _mockAPI;

        private VerifyEmailRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockAPI = new Mock<IAPIVerifyEmailService>();

            _service = new VerifyEmailService(_mockAPI.Object, new Mock<ILogger<VerifyEmailService>>().Object);

            _request = new VerifyEmailRequest();
        }

        [Test()]
        public async Task VerifyEmail_WithBadRequestResponse_ThrowsValidationException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, new ErrorResponse());
            _mockAPI.Setup(s => s.VerifyEmail(_request)).Throws(exception);

            Assert.ThrowsAsync<ValidationFailedException>(() => _service.VerifyEmail(_request));
        }

        [Test()]
        public async Task VerifyEmail_WithUnauthorizedResponse_ThrowsUnauthorizedException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.Unauthorized);
            _mockAPI.Setup(s => s.VerifyEmail(_request)).Throws(exception);

            Assert.ThrowsAsync<UnauthorizedException>(() => _service.VerifyEmail(_request));
        }

        [Test()]
        public async Task VerifyEmail_WithUnknownError_ThrowsException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.InternalServerError);
            _mockAPI.Setup(s => s.VerifyEmail(_request)).Throws(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.VerifyEmail(_request));
        }

        [Test()]
        public void VerifyEmail_WithSuccess_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(() => _service.VerifyEmail(_request));

            _mockAPI.Verify(s => s.VerifyEmail(_request), Times.Once);
        }
    }
}