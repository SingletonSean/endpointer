using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Services.Refresh
{
    public interface IAPIRefreshService
    {
        /// <summary>
        /// Refresh an expired access token with a refresh token.
        /// </summary>
        /// <param name="request">The request containing the refresh token.</param>
        /// <returns>The successful response with the new tokens.</returns>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Post("/")]
        Task<SuccessResponse<AuthenticatedUserResponse>> Refresh([Body] RefreshRequest request);
    }
}
