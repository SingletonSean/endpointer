﻿using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Refit;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IAPILoginService _api;

        public LoginService(IAPILoginService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task<AuthenticatedUserResponse> Login(LoginRequest request)
        {
            try
            {
                SuccessResponse<AuthenticatedUserResponse> response = await _api.Login(request);

                if(response == null || response.Data == null)
                {
                    throw new Exception("Failed to deserialize API response.");
                }

                return response.Data;
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message, ex.InnerException);
                }

                ErrorResponse error = await ex.GetContentAsAsync<ErrorResponse>();
                if (error == null || error.Errors == null || error.Errors.Count() == 0)
                    throw;

                ErrorMessageResponse firstError = error.Errors.First();
                switch (firstError.Code)
                {
                    case ErrorCode.VALIDATION_FAILURE:
                        throw new ValidationFailedException(error.Errors
                            .Where(e => e.Code == ErrorCode.VALIDATION_FAILURE)
                            .Select(e => e.Message));
                    default:
                        throw;
                }
            }
        }
    }
}