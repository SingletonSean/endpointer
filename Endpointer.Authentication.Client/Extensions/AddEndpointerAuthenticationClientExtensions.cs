using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Services;
using Endpointer.Core.Client.Http;
using Endpointer.Core.Client.Services;
using Endpointer.Core.Client.Stores;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Endpointer.Authentication.Client.Extensions
{
    public static class AddEndpointerAuthenticationClientExtensions
    {
        public static IServiceCollection AddEndpointerAuthenticationClient(this IServiceCollection services,
            AuthenticationEndpointsConfiguration endpointConfiguration,
            IAutoRefreshTokenStore tokenStore)
        {
            return services.AddEndpointerAuthenticationClient(endpointConfiguration, (s) => tokenStore);
        }

        public static IServiceCollection AddEndpointerAuthenticationClient(this IServiceCollection services, 
            AuthenticationEndpointsConfiguration endpointConfiguration,
            Func<IServiceProvider, IAutoRefreshTokenStore> getTokenStore)
        {
            RefitSettings refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());

            services.AddRefitClient<IRegisterService>(refitSettings, endpointConfiguration.RegisterEndpoint);
            services.AddRefitClient<ILoginService>(refitSettings, endpointConfiguration.LoginEndpoint);
            services.AddRefitClient<IRefreshService>(refitSettings, endpointConfiguration.RefreshEndpoint);
            services.AddAutoRefreshRefitClient<ILogoutService>(refitSettings, endpointConfiguration.LogoutEndpoint, getTokenStore);

            return services;
        }

        private static IHttpClientBuilder AddRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint) where TService : class
        {
            return services.AddRefitClient<TService>(settings)
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(endpoint));
        }

        private static IHttpClientBuilder AddAutoRefreshRefitClient<TService>(this IServiceCollection services, 
            RefitSettings settings,
            string endpoint,
            Func<IServiceProvider, IAutoRefreshTokenStore> getTokenStore) where TService : class
        {
            return services.AddRefitClient<TService>(settings, endpoint)
                .AddHttpMessageHandler(s => new AutoRefreshHttpMessageHandler(
                    getTokenStore(s), 
                    s.GetRequiredService<IRefreshService>()));
        }
    }
}
