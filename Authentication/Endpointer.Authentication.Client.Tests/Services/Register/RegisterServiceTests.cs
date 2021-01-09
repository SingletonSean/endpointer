using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Client.Tests.Services;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Moq;
using NUnit.Framework;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Tests.Services.Register
{
    [TestFixture]
    public class RegisterServiceTests : APIServiceTests
    {
        private RegisterService _service;

        private Mock<IAPIRegisterService> _mockApi;

        private RegisterRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mockApi = new Mock<IAPIRegisterService>();

            _service = new RegisterService(_mockApi.Object);

            _request = new RegisterRequest()
            {
                Email = "test@gmail.com",
                Username = "test"
            };
        }

        [Test]
        public async Task Register_WithApiExceptionWithNoContent_ThrowsApiException()
        {
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Register(_request));
        }

        [Test]
        public async Task Register_WithApiExceptionWithNullErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse();
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Register(_request));
        }

        [Test]
        public async Task Register_WithApiExceptionWithEmptyErrorMessages_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse() { Errors = new List<ErrorMessageResponse>() };
            ApiException exception = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(exception);

            Assert.ThrowsAsync<ApiException>(() => _service.Register(_request));
        }

        [Test]
        public async Task Register_WithApiExceptionWithPasswordsDoNotMatchErrorCode_ThrowsConfirmPasswordException()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(AuthenticationErrorCode.PASSWORDS_DO_NOT_MATCH, It.IsAny<string>())
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ConfirmPasswordException>(() => _service.Register(_request));
        }

        [Test]
        public async Task Register_WithApiExceptionWithEmailAlreadyExistsErrorCode_ThrowsEmailAlreadyExistsExceptionForEmail()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(AuthenticationErrorCode.EMAIL_ALREADY_EXISTS, It.IsAny<string>())
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(apiException);

            EmailAlreadyExistsException exception = Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _service.Register(_request));

            Assert.AreEqual(_request.Email, exception.Email);
        }

        [Test]
        public async Task Register_WithApiExceptionWithUsernameAlreadyExistsErrorCode_ThrowsUsernameAlreadyExistsExceptionForUsername()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(AuthenticationErrorCode.USERNAME_ALREADY_EXISTS, It.IsAny<string>())
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(apiException);

            UsernameAlreadyExistsException exception = Assert.ThrowsAsync<UsernameAlreadyExistsException>(() => _service.Register(_request));

            Assert.AreEqual(_request.Username, exception.Username);
        }

        [Test]
        public async Task Register_WithApiExceptionWithValidationFailureErrorCode_ThrowsValidationFailedExceptionWithValidationMessages()
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
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(apiException);

            ValidationFailedException exception = Assert.ThrowsAsync<ValidationFailedException>(() => _service.Register(_request));

            Assert.IsTrue(exception.ValidationMessages.Contains(message1));
            Assert.IsTrue(exception.ValidationMessages.Contains(message2));
        }

        [Test]
        public async Task Register_WithApiExceptionWithUnknownErrorCode_ThrowsApiException()
        {
            ErrorResponse responseContent = new ErrorResponse()
            {
                Errors = new List<ErrorMessageResponse>()
                {
                    new ErrorMessageResponse(123456789, "Invalid request."),
                }
            };
            ApiException apiException = await CreateApiException(HttpStatusCode.BadRequest, responseContent);
            _mockApi.Setup(s => s.Register(_request)).ThrowsAsync(apiException);

            Assert.ThrowsAsync<ApiException>(() => _service.Register(_request));
        }

        [Test]
        public void Register_WithSuccess_DoesNotThrow()
        {
            Assert.DoesNotThrowAsync(() => _service.Register(_request));
        }
    }
}
