using System;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Stores
{
    public interface IAutoRefreshTokenStore : IAccessTokenStore
    {
        bool HasAccessToken { get; }
        bool IsAccessTokenExpired { get; }

        Task<string> GetRefreshToken();
        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}
