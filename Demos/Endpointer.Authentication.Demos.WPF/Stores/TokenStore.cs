using Endpointer.Core.Client.Stores;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Demos.WPF.Stores
{
    public class TokenStore : AutoRefreshTokenStoreBase
    {
        private string _accessToken;
        private string _refreshToken;

        public override string AccessToken => _accessToken ?? string.Empty;

        public override Task<string> GetRefreshToken()
        {
            return Task.FromResult(_refreshToken);
        }

        public override Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            AccessTokenExpirationTime = accessTokenExpirationTime;

            return Task.CompletedTask;
        }
    }
}
