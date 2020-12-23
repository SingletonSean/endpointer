﻿using Endpointer.Core.Client.Stores;
using Refit;
using System;

namespace Endpointer.Authentication.Client.Models
{
    public class EndpointerAuthenticationOptionsBuilder
    {
        private RefitSettings _refitSettings;
        private bool _withAutoTokenRefresh;
        private Func<IServiceProvider, IAutoRefreshTokenStore> _getAutoRefreshTokenStore;

        public EndpointerAuthenticationOptionsBuilder()
        {
            _refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());
        }

        public EndpointerAuthenticationOptionsBuilder UseRefitSettings(RefitSettings settings)
        {
            _refitSettings = settings;

            return this;
        }

        public EndpointerAuthenticationOptionsBuilder WithAutoTokenRefresh(IAutoRefreshTokenStore autoRefreshTokenStore)
        {
            return WithAutoTokenRefresh(s => autoRefreshTokenStore);
        }

        public EndpointerAuthenticationOptionsBuilder WithAutoTokenRefresh(Func<IServiceProvider, IAutoRefreshTokenStore> getAutoRefreshTokenStore)
        {
            _withAutoTokenRefresh = true;
            _getAutoRefreshTokenStore = getAutoRefreshTokenStore;

            return this;
        }

        public EndpointerAuthenticationOptions Build()
        {
            return new EndpointerAuthenticationOptions()
            {
                RefitSettings = _refitSettings,
                AutoTokenRefresh = _withAutoTokenRefresh,
                GetAutoRefreshTokenStore = _getAutoRefreshTokenStore
            };
        }
    }
}
