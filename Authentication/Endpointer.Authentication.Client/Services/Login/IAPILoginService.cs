using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Login
{
    public interface IAPILoginService
    {
        /// <summary>
        /// Login a user by getting tokens.
        /// </summary>
        /// <param name="request">The request containing the login information.</param>
        /// <returns>The successful response with the user's tokens.</returns>
        /// <exception cref="ApiException">Thrown if request fails.</returns>
        [Post("/")]
        Task<SuccessResponse<AuthenticatedUserResponse>> Login([Body] LoginRequest request);
    }
}
