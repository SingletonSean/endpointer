using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Endpointer.Core.API.Services.TokenClaimsDecoders
{
    public class TokenHandlerTokenClaimsDecoder : ITokenClaimsDecoder
    {
        private readonly TokenValidationParameters _validationParameters;

        public TokenHandlerTokenClaimsDecoder(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters;
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetClaims(string token)
        {
            try
            {
                return new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out _);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Failed to validate token.", ex);
            }
        }
    }
}
