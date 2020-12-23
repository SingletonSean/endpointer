using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Http
{
    public class HttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly AccessTokenDecoder _tokenDecoder;

        public HttpRequestAuthenticator(AccessTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

        /// <summary>
        /// Authenticate a user from an HTTP request.
        /// </summary>
        /// <param name="request">The request with the authorization header.</param>
        /// <returns>The authenticated user. Null if authentication fails.</returns>
        public async Task<User> Authenticate(HttpRequest request)
        {
            User user = null;

            // Ensure authorization header has a token.
            string rawBearerToken = request.Headers["Authorization"].FirstOrDefault();
            if (rawBearerToken != null && rawBearerToken.StartsWith(BEARER_PREFIX, StringComparison.InvariantCultureIgnoreCase))
            {
                // Validate the token and get the user.
                string token = rawBearerToken.Substring(BEARER_PREFIX.Length);

                try
                {
                    user = await _tokenDecoder.GetUserFromToken(token);
                }
                catch (Exception)
                {
                    user = null;
                }
            }

            return user;
        }
    }
}
