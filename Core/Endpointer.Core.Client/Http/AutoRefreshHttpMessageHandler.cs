using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Client.Stores;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AutoRefreshHttpMessageHandler> _logger;

        public AutoRefreshHttpMessageHandler(IAutoRefreshTokenStore tokenStore,
            IRefreshService refreshService, 
            ILogger<AutoRefreshHttpMessageHandler> logger)
        {
            _tokenStore = tokenStore;
            _refreshService = refreshService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking access token expiration for request.");
            if(!_tokenStore.HasAccessToken || _tokenStore.IsAccessTokenExpired)
            {
                try
                {
                    _logger.LogInformation("Getting refresh token.");
                    string refreshToken = await _tokenStore.GetRefreshToken();

                    _logger.LogInformation("Refreshing access token with refresh token.");
                    AuthenticatedUserResponse response = await _refreshService.Refresh(new RefreshRequest()
                    {
                        RefreshToken = refreshToken
                    });

                    _logger.LogInformation("Setting new access token and refresh tokens.");
                    await _tokenStore.SetTokens(response.AccessToken, response.RefreshToken, response.AccessTokenExpirationTime);

                    _logger.LogInformation("Setting access token for request.");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to refresh access token.");
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
