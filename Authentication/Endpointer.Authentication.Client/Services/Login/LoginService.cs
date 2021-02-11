using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IAPILoginService _api;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IAPILoginService api, ILogger<LoginService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<AuthenticatedUserResponse> Login(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Sending login request.");
                SuccessResponse<AuthenticatedUserResponse> response = await _api.Login(request);

                if(response == null || response.Data == null)
                {
                    _logger.LogError("Response does not contain data.");
                    throw new Exception("Failed to deserialize API response.");
                }

                _logger.LogInformation("Successfully logged in.");
                return response.Data;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Login request failed.");
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized login credentials.");
                    throw new UnauthorizedException(ex.Message, ex);
                }

                ErrorResponse error = await ex.GetContentAsAsync<ErrorResponse>();
                if (error == null || error.Errors == null || error.Errors.Count() == 0)
                {
                    _logger.LogError("Unknown error.");
                    throw;
                }

                ErrorMessageResponse firstError = error.Errors.First();
                switch (firstError.Code)
                {
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
