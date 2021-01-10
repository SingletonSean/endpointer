using Endpointer.Core.Client.Stores;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Endpointer.Core.Client.Http
{
    /// <summary>
    /// Add an access token header to the request.
    /// </summary>
    public class AccessTokenHttpMessageHandler : DelegatingHandler
    {
        private readonly IAccessTokenStore _tokenStore;
        private readonly ILogger<AccessTokenHttpMessageHandler> _logger;

        public AccessTokenHttpMessageHandler(IAccessTokenStore tokenStore, ILogger<AccessTokenHttpMessageHandler> logger)
        {
            _tokenStore = tokenStore;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting access token for request.");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
