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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<AccessTokenDecoder> _logger;

        public AccessTokenDecoder(ITokenClaimsDecoder claimsDecoder,
            TokenValidationParameters tokenValidationParameters,
            ILogger<AccessTokenDecoder> logger)
        {
            _claimsDecoder = claimsDecoder;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<User> GetUserFromToken(string token)
        {
            _logger.LogInformation("Getting claims from token.");
            ClaimsPrincipal claims = _claimsDecoder.GetClaims(token, _tokenValidationParameters);

            _logger.LogInformation("Getting user ID from token claims.");
            Guid id = GetId(claims);

            _logger.LogInformation("Getting user email from token claims.");
            string email = GetEmail(claims);

            _logger.LogInformation("Getting username from token claims.");
            string username = GetUsername(claims);

            _logger.LogInformation("Getting email verification status from token claims.");
            bool isEmailVerified = IsEmailVerified(claims);

            _logger.LogInformation("Successfully decoded user {UserId} from access token.", id);
            return Task.FromResult(new User()
            {
                Id = id,
                Email = email,
                Username = username,
                IsEmailVerified = isEmailVerified
            });
        }

        private bool IsEmailVerified(ClaimsPrincipal claims)
        {
            string rawEmailVerified = claims.FindFirst(ClaimKey.EMAIL_VERIFIED)?.Value;
            if (!bool.TryParse(rawEmailVerified, out bool emailVerified))
            {
                _logger.LogError("Failed to parse email verification status from token claims.");
                return false;
            }

            return emailVerified;
        }

        private string GetUsername(ClaimsPrincipal claims)
        {
            Claim usernameClaim = claims.FindFirst(ClaimTypes.Name);
            if (usernameClaim == null)
            {
                _logger.LogError("Failed to parse username from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse username from JWT.");
            }

            return usernameClaim.Value;
        }

        private string GetEmail(ClaimsPrincipal claims)
        {
            Claim emailClaim = claims.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                _logger.LogError("Failed to parse user email from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse email from JWT.");
            }

            return emailClaim.Value;
        }

        private Guid GetId(ClaimsPrincipal claims)
        {
            string rawId = claims.FindFirst("id")?.Value;
            if (!Guid.TryParse(rawId, out Guid id))
            {
                _logger.LogError("Failed to parse user id from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse ID from JWT.");
            }

            return id;
        }
    }
}
