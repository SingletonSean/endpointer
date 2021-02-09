using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Endpointer.Core.API.Exceptions;

namespace Endpointer.Core.API.Http
{
    public interface IHttpRequestAuthenticator
    {
        /// <summary>
        /// Authenticate a user from an HTTP request.
        /// </summary>
        /// <param name="request">The request with the authorization header.</param>
        /// <returns>The authenticated user.</returns>
        /// <exception cref="BearerSchemeNotProvidedException">Thrown if Authorization header does not have 'Bearer ' prefix.</exception>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if unable to get user values from token.</exception>
        /// <exception cref="UnverifiedEmailException">Thrown if user has an unverified email.</exception>
        Task<User> Authenticate(HttpRequest request);
    }
}