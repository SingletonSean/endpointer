using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Endpointer.Core.API.Services.TokenClaimsDecoders
{
    public class TokenHandlerTokenClaimsDecoder : ITokenClaimsDecoder
    {
        private readonly TokenValidationParameters _validationParameters;
        private readonly ILogger<TokenHandlerTokenClaimsDecoder> _logger;

        public TokenHandlerTokenClaimsDecoder(TokenValidationParameters validationParameters, ILogger<TokenHandlerTokenClaimsDecoder> logger)
        {
            _validationParameters = validationParameters;
            _logger = logger;
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetClaims(string token)
        {
            try
            {
                _logger.LogInformation("Validating token.");
                ClaimsPrincipal claims = new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out _);

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
