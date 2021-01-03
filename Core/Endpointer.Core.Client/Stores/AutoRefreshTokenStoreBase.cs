using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Stores
{
    public abstract class AutoRefreshTokenStoreBase : IAutoRefreshTokenStore
    {
        public bool HasAccessToken => !string.IsNullOrEmpty(AccessToken);
        public bool IsAccessTokenExpired => DateTime.UtcNow >= AccessTokenExpirationTime;
        protected DateTime AccessTokenExpirationTime { get; set; }

        public abstract string AccessToken { get; }
        public abstract Task<string> GetRefreshToken();
        public abstract Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}
