using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Client.Stores;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Http
{
    /// <summary>
    /// Refresh the access token for the request.
    /// </summary>
    public class AutoRefreshHttpMessageHandler : DelegatingHandler
    {
        private readonly IAutoRefreshTokenStore _tokenStore;
        private readonly IRefreshService _refreshService;

        public AutoRefreshHttpMessageHandler(IAutoRefreshTokenStore tokenStore, 
            IRefreshService refreshService)
        {
            _tokenStore = tokenStore;
            _refreshService = refreshService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(!_tokenStore.HasAccessToken || _tokenStore.IsAccessTokenExpired)
            {
                try
                {
                    string refreshToken = await _tokenStore.GetRefreshToken();

                    AuthenticatedUserResponse response = await _refreshService.Refresh(new RefreshRequest()
                    {
                        RefreshToken = refreshToken
                    });

                    await _tokenStore.SetTokens(response.AccessToken, response.RefreshToken, response.AccessTokenExpirationTime);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken);
                }
                catch (Exception)
                {
                    throw await CreateUnauthorizedException(request);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private static async Task<ApiException> CreateUnauthorizedException(HttpRequestMessage request)
        {
            return await ApiException.Create(request, HttpMethod.Post, new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }
    }
}
