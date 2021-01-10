using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Services.TokenDecoders
{
    public class AccessTokenDecoder : IAccessTokenDecoder
    {
        private readonly ITokenClaimsDecoder _claimsDecoder;
        private readonly ILogger<AccessTokenDecoder> _logger;

        public AccessTokenDecoder(ITokenClaimsDecoder claimsDecoder, ILogger<AccessTokenDecoder> logger)
        {
            _claimsDecoder = claimsDecoder;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<User> GetUserFromToken(string token)
        {
            _logger.LogInformation("Getting claims from token.");
            ClaimsPrincipal claims = _claimsDecoder.GetClaims(token);

            _logger.LogInformation("Getting user ID from token claims.");
            string rawId = claims.FindFirst("id")?.Value;
            if (!Guid.TryParse(rawId, out Guid id))
            {
                _logger.LogError("Failed to parse user id from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse ID from JWT.");
            }

            _logger.LogInformation("Getting user email from token claims.");
            Claim emailClaim = claims.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                _logger.LogError("Failed to parse user email from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse email from JWT.");
            }

            _logger.LogInformation("Getting username from token claims.");
            Claim usernameClaim = claims.FindFirst(ClaimTypes.Name);
            if (usernameClaim == null)
            {
                _logger.LogError("Failed to parse username from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse username from JWT.");
            }

            _logger.LogInformation("Successfully decoded user {UserId} from access token.", id);
            return Task.FromResult(new User()
            {
                Id = id,
                Email = emailClaim.Value,
                Username = usernameClaim.Value
            });
        }
    }
}
