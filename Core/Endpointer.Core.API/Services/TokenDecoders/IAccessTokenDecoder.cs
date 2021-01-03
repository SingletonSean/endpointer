using Endpointer.Core.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Services.TokenDecoders
{
    public interface IAccessTokenDecoder
    {
        /// <summary>
        /// Get a user from a JWT token.
        /// </summary>
        /// <param name="token">The JWT token value.</param>
        /// <returns>The user signed into the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if unable to get claims from token.</exception>
        /// <exception cref="SecurityTokenDecryptionFailedException">Thrown if unable to get user values from token.</exception>
        Task<User> GetUserFromToken(string token);
    }
}