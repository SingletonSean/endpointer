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
            AuthenticationEndpointsConfiguration endpointsConfiguration,
            Func<IServiceProvider, IAccessTokenStore> getTokenStore,
            Func<EndpointerAuthenticationOptionsBuilder, EndpointerAuthenticationOptionsBuilder> configureOptions = null)
        {
            EndpointerAuthenticationOptionsBuilder optionsBuilder = new EndpointerAuthenticationOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerAuthenticationOptions options = optionsBuilder.Build();

            services.AddRefitClient<IRegisterService>(options.RefitSettings, endpointsConfiguration.RegisterEndpoint);
            services.AddRefitClient<ILoginService>(options.RefitSettings, endpointsConfiguration.LoginEndpoint);
            services.AddRefitClient<IRefreshService>(options.RefitSettings, endpointsConfiguration.RefreshEndpoint);

            if(options.AutoTokenRefresh)
            {
                services.AddAutoRefreshRefitClient<ILogoutService>(options.RefitSettings, 
                    endpointsConfiguration.LogoutEndpoint, 
                    options.GetAutoRefreshTokenStore);
            }
            else
            {
                services.AddAccessTokenRefitClient<ILogoutService>(options.RefitSettings,
                    endpointsConfiguration.LogoutEndpoint,
                    getTokenStore);
            }

            return services;
        }

        private static IHttpClientBuilder AddRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint) where TService : class
        {
            return services.AddRefitClient<TService>(settings)
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(endpoint));
        }

        private static IHttpClientBuilder AddAccessTokenRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint,
            Func<IServiceProvider, IAccessTokenStore> getTokenStore) where TService : class
        {
            return services.AddRefitClient<TService>(settings)
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(endpoint))
                .AddHttpMessageHandler(s => new AccessTokenHttpMessageHandler(getTokenStore(s)));
        }

        private static IHttpClientBuilder AddAutoRefreshRefitClient<TService>(this IServiceCollection services, 
            RefitSettings settings,
            string endpoint,
            Func<IServiceProvider, IAutoRefreshTokenStore> getRefreshTokenStore) where TService : class
        {
            return services.AddRefitClient<TService>(settings, endpoint)
                .AddHttpMessageHandler(s => new AutoRefreshHttpMessageHandler(
                    getRefreshTokenStore(s),
                    s.GetRequiredService<IRefreshService>()))
                .AddHttpMessageHandler(s => new AccessTokenHttpMessageHandler(getRefreshTokenStore(s)));
        }
    }
}
