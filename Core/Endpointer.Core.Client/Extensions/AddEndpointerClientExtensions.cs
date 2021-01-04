using Endpointer.Core.Client.Http;
using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Client.Stores;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Endpointer.Core.Client.Extensions
{
    public static class AddEndpointerClientExtensions
    {
        /// <summary>
        /// Register a Refit service.
        /// </summary>
        /// <typeparam name="TService">Type of service to register.</typeparam>
        /// <param name="services">Service collection to register with.</param>
        /// <param name="settings">Settings for the registered service.</param>
        /// <param name="endpoint">Base URL of the service.</param>
        /// <returns>Builder for the HttpClient used to contact the service.</returns>
        public static IHttpClientBuilder AddRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint) where TService : class
        {
            return services.AddRefitClient<TService>(settings)
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(endpoint));
        }

        /// <summary>
        /// Register a Refit service with access token authorization.
        /// </summary>
        /// <typeparam name="TService">Type of service to register.</typeparam>
        /// <param name="services">Service collection to register with.</param>
        /// <param name="settings">Settings for the registered service.</param>
        /// <param name="endpoint">Base URL of the service.</param>
        /// <param name="getTokenStore">Function to get the store containing the access token.</param>
        /// <returns>Builder for the HttpClient used to contact the service.</returns>
        public static IHttpClientBuilder AddAccessTokenRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint,
            Func<IServiceProvider, IAccessTokenStore> getTokenStore) where TService : class
        {
            return services.AddRefitClient<TService>(settings, endpoint)
                .AddHttpMessageHandler(s => new AccessTokenHttpMessageHandler(getTokenStore(s)));
        }

        /// <summary>
        /// Register a Refit service with access token authorization and automatic token refresh.
        /// </summary>
        /// <typeparam name="TService">Type of service to register.</typeparam>
        /// <param name="services">Service collection to register with.</param>
        /// <param name="settings">Settings for the registered service.</param>
        /// <param name="endpoint">Base URL of the service.</param>
        /// <param name="getRefreshTokenStore">Function to get the store containing the access and refresh tokens.</param>
        /// <returns>Builder for the HttpClient used to contact the service.</returns>
        public static IHttpClientBuilder AddAutoRefreshRefitClient<TService>(this IServiceCollection services, 
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
