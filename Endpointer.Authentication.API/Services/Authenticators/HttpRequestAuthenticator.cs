using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.Authenticators
{
    public class HttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly AccessTokenDecoder _tokenDecoder;

        public HttpRequestAuthenticator(AccessTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

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
