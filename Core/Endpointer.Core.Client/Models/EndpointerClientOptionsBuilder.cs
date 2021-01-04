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

        /// <summary>
        /// Set the Refit settings for the client.
        /// </summary>
        /// <param name="settings">The Refit settings to use.</param>
        /// <returns>The configured builder.</returns>
        public EndpointerClientOptionsBuilder UseRefitSettings(RefitSettings settings)
        {
            _refitSettings = settings;

            return this;
        }

        /// <summary>
        /// Add automatic token refresh.
        /// </summary>
        /// <param name="autoRefreshTokenStore">The store to handle tokens.</param>
        /// <returns>The configured builder.</returns>
        public EndpointerClientOptionsBuilder WithAutoTokenRefresh(IAutoRefreshTokenStore autoRefreshTokenStore)
        {
            return WithAutoTokenRefresh(s => autoRefreshTokenStore);
        }

        /// <summary>
        /// Add automatic token refresh.
        /// </summary>
        /// <param name="getAutoRefreshTokenStore">The function to get the store to handle tokens.</param>
        /// <returns>The configured builder.</returns>
        public EndpointerClientOptionsBuilder WithAutoTokenRefresh(Func<IServiceProvider, IAutoRefreshTokenStore> getAutoRefreshTokenStore)
        {
            _withAutoTokenRefresh = true;
            _getAutoRefreshTokenStore = getAutoRefreshTokenStore;

            return this;
        }

        /// <summary>
        /// Build the client options.
        /// </summary>
        /// <returns>The built client options.</returns>
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
