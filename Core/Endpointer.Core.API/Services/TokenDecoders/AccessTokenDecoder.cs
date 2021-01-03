using Endpointer.Core.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Services.TokenDecoders
{
    public class AccessTokenDecoder : IAccessTokenDecoder
    {
        private readonly ITokenClaimsDecoder _claimsDecoder;

        public AccessTokenDecoder(ITokenClaimsDecoder claimsDecoder)
        {
            _claimsDecoder = claimsDecoder;
        }

        /// <inheritdoc />
        public Task<User> GetUserFromToken(string token)
        {
            ClaimsPrincipal claims = _claimsDecoder.GetClaims(token);

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
    }
}
