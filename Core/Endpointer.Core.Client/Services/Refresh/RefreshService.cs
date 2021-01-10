﻿using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Services.Refresh
{
    public class RefreshService : IRefreshService
    {
        private readonly IAPIRefreshService _api;
        private readonly ILogger<RefreshService> _logger;

        public RefreshService(IAPIRefreshService api, ILogger<RefreshService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<AuthenticatedUserResponse> Refresh(RefreshRequest request)
        {
            try
            {
                _logger.LogInformation("Sending refresh request.");
                SuccessResponse<AuthenticatedUserResponse> response = await _api.Refresh(request);

                _logger.LogInformation("Ensuring response data exists.");
                if (response == null || response.Data == null)
                {
                    _logger.LogError("Response does not contain data.");
                    throw new Exception("Failed to deserialize API response.");
                }

                _logger.LogInformation("Successfully refreshed.");
                return response.Data;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Refresh request failed.");
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError("Invalid refresh token.");
                    throw new InvalidRefreshTokenException();
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
                    case ErrorCode.INVALID_REFRESH_TOKEN:
                        _logger.LogError("Invalid refresh token.");
                        throw new InvalidRefreshTokenException();
                    default:
                        _logger.LogError("Unknown error.");
                        throw;
                }
            }
        }
    }
}
