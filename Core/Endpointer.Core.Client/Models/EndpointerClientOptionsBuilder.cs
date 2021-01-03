using Endpointer.Core.Client.Stores;
using Refit;
using System;

namespace Endpointer.Core.Client.Models
{
    public class EndpointerClientOptionsBuilder
    {
        private RefitSettings _refitSettings;
        private bool _withAutoTokenRefresh;
        private Func<IServiceProvider, IAutoRefreshTokenStore> _getAutoRefreshTokenStore;

        public EndpointerClientOptionsBuilder()
        {
            _refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());
        }

        public EndpointerClientOptionsBuilder UseRefitSettings(RefitSettings settings)
        {
            _refitSettings = settings;

            return this;
        }

        public EndpointerClientOptionsBuilder WithAutoTokenRefresh(IAutoRefreshTokenStore autoRefreshTokenStore)
        {
            return WithAutoTokenRefresh(s => autoRefreshTokenStore);
        }

        public EndpointerClientOptionsBuilder WithAutoTokenRefresh(Func<IServiceProvider, IAutoRefreshTokenStore> getAutoRefreshTokenStore)
        {
            _withAutoTokenRefresh = true;
            _getAutoRefreshTokenStore = getAutoRefreshTokenStore;

            return this;
        }

        public EndpointerClientOptions Build()
        {
            return new EndpointerClientOptions()
            {
                RefitSettings = _refitSettings,
                AutoTokenRefresh = _withAutoTokenRefresh,
                GetAutoRefreshTokenStore = _getAutoRefreshTokenStore
            };
        }
    }
}
