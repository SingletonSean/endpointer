using Endpointer.Core.Client.Stores;
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

        public AccessTokenHttpMessageHandler(IAccessTokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
