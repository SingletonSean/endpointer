using Endpointer.Authentication.Client.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Demos.WPF.Stores
{
    public class TokenStore : IAutoRefreshTokenStore
    {
        private string _accessToken;
        private string _refreshToken;
        private DateTime _accessTokenExpirationTime;

        public string AccessToken => _accessToken;
        public bool IsAccessTokenExpired => DateTime.UtcNow >= _accessTokenExpirationTime;

        public Task<string> GetRefreshToken()
        {
            return Task.FromResult(_refreshToken);
        }

        public Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _accessTokenExpirationTime = accessTokenExpirationTime;

            return Task.CompletedTask;
        }
    }
}
