using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Endpointer.Core.API.Services.TokenClaimsDecoders
{
    public interface ITokenClaimsDecoder
    {
        /// <summary>
        /// Get claims signed into a JWT token.
        /// </summary>
        /// <param name="token">The token value.</param>
        /// <returns>The claims signed into the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        ClaimsPrincipal GetClaims(string token);
    }
}
