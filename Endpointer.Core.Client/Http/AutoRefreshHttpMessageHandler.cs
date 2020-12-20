using Endpointer.Core.Client.Services;
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
    public class AutoRefreshHttpMessageHandler : DelegatingHandler
    {
        private readonly IAutoRefreshTokenStore _tokenStore;
        private readonly IRefreshService _refreshService;

        public AutoRefreshHttpMessageHandler(IAutoRefreshTokenStore tokenStore, IRefreshService refreshService)
        {
            _tokenStore = tokenStore;
            _refreshService = refreshService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_tokenStore.IsAccessTokenExpired)
            {
                try
                {
                    string refreshToken = await _tokenStore.GetRefreshToken();

                    SuccessResponse<AuthenticatedUserResponse> response = await _refreshService.Refresh(new RefreshRequest()
                    {
                        RefreshToken = refreshToken
                    });
                    AuthenticatedUserResponse userResponse = response.Data;

                    if(userResponse == null)
                    {
                        throw await CreateUnauthorizedException(request);
                    }

                    await _tokenStore.SetTokens(userResponse.AccessToken, userResponse.RefreshToken, userResponse.AccessTokenExpirationTime);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.AccessToken);
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
