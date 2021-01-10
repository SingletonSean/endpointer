using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Endpointer.Core.API.Exceptions;
using Microsoft.Extensions.Logging;

namespace Endpointer.Core.API.Http
{
    public class HttpRequestAuthenticator : IHttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly IAccessTokenDecoder _tokenDecoder;
        private readonly ILogger<HttpRequestAuthenticator> _logger;

        public HttpRequestAuthenticator(IAccessTokenDecoder tokenDecoder, ILogger<HttpRequestAuthenticator> logger)
        {
            _tokenDecoder = tokenDecoder;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> Authenticate(HttpRequest request)
        {
            _logger.LogInformation("Getting authorization header value.");
            string authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

            _logger.LogInformation("Ensuring authorization header has bearer prefix value.");
            if (string.IsNullOrEmpty(authorizationHeader) || !HasBearerPrefix(authorizationHeader))
            {
                _logger.LogError("No bearer authorization value provided.");
                throw new BearerSchemeNotProvidedException(authorizationHeader);
            }

            _logger.LogInformation("Getting token from authorization header.");
            string token = authorizationHeader.Substring(BEARER_PREFIX.Length);

            _logger.LogInformation("Getting user from token.");
            User user =  await _tokenDecoder.GetUserFromToken(token);

            _logger.LogInformation("Successfully decoded user {UserId} from token.", user.Id);
            return user;
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
