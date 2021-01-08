using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Endpointer.Core.API.Exceptions;

namespace Endpointer.Core.API.Http
{
    public class HttpRequestAuthenticator : IHttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly IAccessTokenDecoder _tokenDecoder;

        public HttpRequestAuthenticator(IAccessTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

        /// <inheritdoc />
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
