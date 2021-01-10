using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IAPIRegisterService _api;
        private readonly ILogger<RegisterService> _logger;

        public RegisterService(IAPIRegisterService api, ILogger<RegisterService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Register(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Sending register request.");
                await _api.Register(request);
                _logger.LogInformation("Successfully registered.");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Register request failed.");
                
                ErrorResponse error = await ex.GetContentAsAsync<ErrorResponse>();
                if (error == null || error.Errors == null || error.Errors.Count() == 0)
                {
                    _logger.LogError("Unknown error.");
                    throw;
                }

                ErrorMessageResponse firstError = error.Errors.First();
                switch(firstError.Code)
                {
                    case AuthenticationErrorCode.PASSWORDS_DO_NOT_MATCH:
                        _logger.LogError("Password does not match confirm password.");
                        throw new ConfirmPasswordException();
                    case AuthenticationErrorCode.EMAIL_ALREADY_EXISTS:
                        _logger.LogError("Email already exists.");
                        throw new EmailAlreadyExistsException(request.Email);
                    case AuthenticationErrorCode.USERNAME_ALREADY_EXISTS:
                        _logger.LogError("Username already exists.");
                        throw new UsernameAlreadyExistsException(request.Username);
                    case ErrorCode.VALIDATION_FAILURE:
                        IEnumerable<string> validationMessages = error.Errors
                            .Where(e => e.Code == ErrorCode.VALIDATION_FAILURE)
                            .Select(e => e.Message);
                        _logger.LogError("Validation failed.", string.Join(",", validationMessages));
                        throw new ValidationFailedException(validationMessages);
                    default:
                        _logger.LogError("Unknown error code.");
                        throw;
                }
            }
        }
    }
}
