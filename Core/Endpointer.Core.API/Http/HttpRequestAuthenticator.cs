using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Endpointer.Core.API.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Endpointer.Core.API.Http
{
    public class HttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly IAccessTokenDecoder _tokenDecoder;

        public HttpRequestAuthenticator(IAccessTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

        /// <summary>
        /// Authenticate a user from an HTTP request.
        /// </summary>
        /// <param name="request">The request with the authorization header.</param>
        /// <returns>The authenticated user.</returns>
        /// <exception cref="BearerSchemeNotProvidedException">Thrown if Authorization header does not have 'Bearer ' prefix.</exception>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if unable to get user values from token.</exception>
        public async Task<User> Authenticate(HttpRequest request)
        {
            string authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader) || !HasBearerPrefix(authorizationHeader))
            {
                throw new BearerSchemeNotProvidedException(authorizationHeader);
            }

            string token = authorizationHeader.Substring(BEARER_PREFIX.Length);

            return await _tokenDecoder.GetUserFromToken(token);
        }

        /// <summary>
        /// Check if a string started with a bearer prefix.
        /// </summary>
        /// <param name="authorizationHeader">The string to check.</param>
        /// <returns>True/false for if the string starts with a bearer prefix.</returns>
        private static bool HasBearerPrefix(string authorizationHeader)
        {
            return authorizationHeader.StartsWith(BEARER_PREFIX, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
