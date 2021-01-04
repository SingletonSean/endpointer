using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Moq;
using NUnit.Framework;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Tests.Services.Refresh
{
    [TestFixture]
    public class RefreshServiceTests
    {
        private RefreshService _service;

        private Mock<IAPIRefreshService> _mockApi;

        private RefreshRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IAPIRefreshService>();

            _service = new RefreshService(_mockApi.Object);

            _request = new RefreshRequest();
        }

        [Test]
        public void Refresh_WithNullResponse_ThrowsException()
        {
            _mockApi.Setup(s => s.Refresh(_request)).ReturnsAsync(() => null);

            Assert.ThrowsAsync<Exception>(() => _service.Refresh(_request));
        }

        [Test]
        public void Refresh_WithNullResponseData_ThrowsException()
        {
            _mockApi.Setup(s => s.Refresh(_request)).ReturnsAsync(() => new SuccessResponse<AuthenticatedUserResponse>(null));

            Assert.ThrowsAsync<Exception>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithNotFoundApiException_ThrowsInvalidRefreshTokenException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.NotFound);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<InvalidRefreshTokenException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithNoContent_ThrowsApiException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithNullErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse();
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithEmptyErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse() { Errors = new List<ErrorMessageResponse>() };
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithValidationFailureErrorCode_ThrowsValidationFailedExceptionWithValidationMessages()
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
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(apiException);

            ValidationFailedException exception = Assert.ThrowsAsync<ValidationFailedException>(() => _service.Refresh(_request));

            Assert.IsTrue(exception.ValidationMessages.Contains(message1));
            Assert.IsTrue(exception.ValidationMessages.Contains(message2));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithInvalidRefreshTokenErrorCode_ThrowsInvalidRefreshTokenException()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(ErrorCode.INVALID_REFRESH_TOKEN, "Invalid refresh token."),
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<InvalidRefreshTokenException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithApiExceptionWithUnknownErrorCode_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(123456789, "Invalid refresh token."),
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Refresh(_request)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ApiException>(() => _service.Refresh(_request));
        }

        [Test]
        public async Task Refresh_WithSuccess_ReturnsData()
        {
            AuthenticatedUserResponse expected = new AuthenticatedUserResponse()
            {
                AccessToken = "123test123",
                RefreshToken = "123refresg123",
                AccessTokenExpirationTime = DateTime.Now.AddDays(1)
            };
            _mockApi.Setup(s => s.Refresh(_request)).ReturnsAsync(new SuccessResponse<AuthenticatedUserResponse>(expected));

            AuthenticatedUserResponse response = await _service.Refresh(_request);

            Assert.AreEqual(expected.AccessToken, response.AccessToken);
            Assert.AreEqual(expected.RefreshToken, response.RefreshToken);
            Assert.AreEqual(expected.AccessTokenExpirationTime, response.AccessTokenExpirationTime);
        }

        private async Task<ApiException> CreateApiException(
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError, 
            object content = null)
        {
            return await ApiException.Create(null, 
                HttpMethod.Post, 
                new HttpResponseMessage(statusCode)
                {
                    Content = JsonContent.Create(content),
                },
                new RefitSettings(new NewtonsoftJsonContentSerializer()));
        }
    }
}
