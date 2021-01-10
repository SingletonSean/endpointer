using System;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public interface IRefreshTokenGenerator
    {
        /// <summary>
        /// Generate a refresh token.
        /// </summary>
        /// <returns>The generated refresh token value.</returns>
        /// <exception cref="Exception">Thrown if token generation fails.</exception>
        string GenerateToken();
    }
}