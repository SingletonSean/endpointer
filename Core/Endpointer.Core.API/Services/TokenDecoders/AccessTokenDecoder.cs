using Endpointer.Core.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Services.TokenDecoders
{
    public class AccessTokenDecoder
    {
        private readonly TokenValidationParameters _validationParameters;

        public AccessTokenDecoder(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters;
        }

        /// <summary>
        /// Get a user from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token value.</param>
        /// <returns>The user signed into the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if unable to get user values from token.</exception>
        public Task<User> GetUserFromToken(string token)
        {
            ClaimsPrincipal claims = GetUserClaims(token);

            string rawId = claims.FindFirst("id")?.Value;
            if (!Guid.TryParse(rawId, out Guid id))
            {
                throw new SecurityTokenDecryptionFailedException("Unable to parse ID from JWT.");
            }

            Claim emailClaim = claims.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                throw new SecurityTokenDecryptionFailedException("Unable to parse email from JWT.");
            }

            Claim usernameClaim = claims.FindFirst(ClaimTypes.Name);
            if (usernameClaim == null)
            {
                throw new SecurityTokenDecryptionFailedException("Unable to parse username from JWT.");
            }

            return Task.FromResult(new User()
            {
                Id = id,
                Email = emailClaim.Value,
                Username = usernameClaim.Value
            });
        }

        /// <summary>
        /// Get claims signed into a JWT token.
        /// </summary>
        /// <param name="token">The token value.</param>
        /// <returns>The claims signed into the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        private ClaimsPrincipal GetUserClaims(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                return handler.ValidateToken(token, _validationParameters, out _);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException(string.Empty, ex);
            }
        }
    }
}
