using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Services.Refresh
{
    public interface IRefreshService
    {
        /// <summary>
        /// Refresh an expired access token with a refresh token.
        /// </summary>
        /// <param name="request">The request containing the refresh token.</param>
        /// <returns>The response with the new tokens.</returns>
        /// <exception cref="InvalidRefreshTokenException">Thrown if refresh token is invalid or expired.</exception>
        /// <exception cref="ValidationFailedException">Thrown if request has validation errors.</exception>
        /// <exception cref="Exception">Thrown if refresh fails.</exception>
        Task<AuthenticatedUserResponse> Refresh(RefreshRequest request);
    }
}
