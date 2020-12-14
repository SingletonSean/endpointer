using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Stores
{
    public interface IAutoRefreshTokenStore
    {
        string AccessToken { get; }
        bool IsAccessTokenExpired { get; }

        Task<string> GetRefreshToken();
        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}
