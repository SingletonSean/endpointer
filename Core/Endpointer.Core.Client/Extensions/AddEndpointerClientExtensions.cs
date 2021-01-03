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
        public static IHttpClientBuilder AddRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint) where TService : class
        {
            return services.AddRefitClient<TService>(settings)
                .ConfigureHttpClient(o => o.BaseAddress = new Uri(endpoint));
        }

        public static IHttpClientBuilder AddAccessTokenRefitClient<TService>(this IServiceCollection services,
            RefitSettings settings,
            string endpoint,
            Func<IServiceProvider, IAccessTokenStore> getTokenStore) where TService : class
        {
            return services.AddRefitClient<TService>(settings, endpoint)
                .AddHttpMessageHandler(s => new AccessTokenHttpMessageHandler(getTokenStore(s)));
        }

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
