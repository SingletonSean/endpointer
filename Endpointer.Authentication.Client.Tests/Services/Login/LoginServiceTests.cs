using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Moq;
using NUnit.Framework;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Tests.Services.Login
{
    [TestFixture]
    public class LoginServiceTests : APIServiceTests
    {
        private LoginService _service;

        private Mock<IAPILoginService> _mockApi;

        private LoginRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IAPILoginService>();

            _service = new LoginService(_mockApi.Object);

            _request = new LoginRequest();
        }

        [Test]
        public void Login_WithNullResponse_ThrowsException()
        {
            _mockApi.Setup(s => s.Login(_request)).ReturnsAsync(() => null);

            Assert.ThrowsAsync<Exception>(() => _service.Login(_request));
        }

        [Test]
        public void Login_WithNullResponseData_ThrowsException()
        {
            _mockApi.Setup(s => s.Login(_request)).ReturnsAsync(() => new SuccessResponse<AuthenticatedUserResponse>(null));

            Assert.ThrowsAsync<Exception>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithUnauthorizedApiException_ThrowsUnauthorizedException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.Unauthorized);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<UnauthorizedException>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithApiExceptionWithNoContent_ThrowsApiException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithApiExceptionWithNullErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse();
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithApiExceptionWithEmptyErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse() { Errors = new List<ErrorMessageResponse>() };
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithApiExceptionWithValidationFailureErrorCode_ThrowsValidationFailedExceptionWithValidationMessages()
        {
            string message1 = "Validation error 1";
            string message2 = "Validation error 2";
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(ErrorCode.VALIDATION_FAILURE, message1),
                    new ErrorMessageResponse(ErrorCode.VALIDATION_FAILURE, message2),
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(apiException);

            ValidationFailedException exception = Assert.ThrowsAsync<ValidationFailedException>(() => _service.Login(_request));

            Assert.IsTrue(exception.ValidationMessages.Contains(message1));
            Assert.IsTrue(exception.ValidationMessages.Contains(message2));
        }

        [Test]
        public async Task Login_WithApiExceptionWithUnknownErrorCode_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(123456789, "Invalid credentials."),
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Login(_request)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ApiException>(() => _service.Login(_request));
        }

        [Test]
        public async Task Login_WithSuccess_ReturnsData()
        {
            AuthenticatedUserResponse expected = new AuthenticatedUserResponse()
            {
                AccessToken = "123test123",
                RefreshToken = "123refresh123",
                AccessTokenExpirationTime = DateTime.Now.AddDays(1)
            };
            _mockApi.Setup(s => s.Login(_request)).ReturnsAsync(new SuccessResponse<AuthenticatedUserResponse>(expected));

            AuthenticatedUserResponse response = await _service.Login(_request);

            Assert.AreEqual(expected.AccessToken, response.AccessToken);
            Assert.AreEqual(expected.RefreshToken, response.RefreshToken);
            Assert.AreEqual(expected.AccessTokenExpirationTime, response.AccessTokenExpirationTime);
        }
    }
}
