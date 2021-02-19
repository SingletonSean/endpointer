using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Endpointer.Core.API.Services.TokenClaimsDecoders
{
    public class TokenHandlerTokenClaimsDecoder : ITokenClaimsDecoder
    {
        private readonly ILogger<TokenHandlerTokenClaimsDecoder> _logger;

        public TokenHandlerTokenClaimsDecoder(ILogger<TokenHandlerTokenClaimsDecoder> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetClaims(string token, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                _logger.LogInformation("Validating token.");
                ClaimsPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out _);

                _logger.LogInformation("Successfully retrieved claims from token.");
                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                throw new SecurityTokenException("Failed to validate token.", ex);
            }
        }
    }
}
