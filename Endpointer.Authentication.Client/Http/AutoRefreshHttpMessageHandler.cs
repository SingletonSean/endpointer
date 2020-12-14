using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Client.Stores;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Http
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
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }

                    await _tokenStore.SetTokens(userResponse.AccessToken, userResponse.RefreshToken, userResponse.AccessTokenExpirationTime);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.AccessToken);
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
