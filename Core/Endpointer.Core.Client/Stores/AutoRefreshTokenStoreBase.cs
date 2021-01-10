using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Stores
{
    public abstract class AutoRefreshTokenStoreBase : IAutoRefreshTokenStore
    {
        /// <inheritdoc/>
        public bool HasAccessToken => !string.IsNullOrEmpty(AccessToken);

        /// <inheritdoc/>
        public bool IsAccessTokenExpired => DateTime.UtcNow >= AccessTokenExpirationTime;
        
        /// <summary>
        /// The access token's expiration time.
        /// </summary>
        protected virtual DateTime AccessTokenExpirationTime { get; set; }

        /// <inheritdoc/>
        public abstract string AccessToken { get; }

        /// <inheritdoc/>
        public abstract Task<string> GetRefreshToken();

        /// <inheritdoc/>
        public abstract Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}
