using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Stores
{
    public interface IAutoRefreshTokenStore : IAccessTokenStore
    {
        /// <summary>
        /// Check if store has an access token.
        /// </summary>
        bool HasAccessToken { get; }

        /// <summary>
        /// Check if store access token is expired.
        /// </summary>
        bool IsAccessTokenExpired { get; }

        /// <summary>
        /// Get refresh token from store.
        /// </summary>
        /// <returns>The stored refresh token.</returns>
        Task<string> GetRefreshToken();

        /// <summary>
        /// Set new token values.
        /// </summary>
        /// <param name="accessToken">The new access token.</param>
        /// <param name="refreshToken">The new refresh token.</param>
        /// <param name="accessTokenExpirationTime">The access token's expiration time.</param>
        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}
