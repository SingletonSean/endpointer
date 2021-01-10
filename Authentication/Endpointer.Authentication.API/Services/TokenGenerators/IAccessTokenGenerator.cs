using Endpointer.Core.API.Models;
using System;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public interface IAccessTokenGenerator
    {
        /// <summary>
        /// Generate an access token for a user.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="expirationTime">The expiration time of the access token.</param>
        /// <returns>The generated access token.</returns>
        /// <exception cref="Exception">Thrown if token generation fails.</exception>
        string GenerateToken(User user, DateTime expirationTime);
    }
}