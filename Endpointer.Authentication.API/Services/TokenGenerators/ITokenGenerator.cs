using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generate a JWT token.
        /// </summary>
        /// <param name="secretKey">The token's signing key.</param>
        /// <param name="issuer">The token issuer.</param>
        /// <param name="audience">The token audience.</param>
        /// <param name="expires">The expiration time of the token.</param>
        /// <param name="claims">The claims to sign into the token.</param>
        /// <returns>The generated token.</returns>
        /// <exception cref="Exception">Thrown if token generation fails.</exception>
        string GenerateToken(string secretKey,
            string issuer, 
            string audience, 
            DateTime expires, 
            IEnumerable<Claim> claims = null);
    }
}