using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Services.Refresh
{
    public class RefreshService : IRefreshService
    {
        private readonly IAPIRefreshService _api;

        public RefreshService(IAPIRefreshService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task<AuthenticatedUserResponse> Refresh(RefreshRequest request)
        {
            try
            {
                SuccessResponse<AuthenticatedUserResponse> response = await _api.Refresh(request);

                if (response == null || response.Data == null)
                {
                    throw new Exception("Failed to deserialize API response.");
                }

                return response.Data;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new InvalidRefreshTokenException();
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
                    case ErrorCode.INVALID_REFRESH_TOKEN:
                        throw new InvalidRefreshTokenException();
                    default:
                        throw;
                }
            }
        }
    }
}
