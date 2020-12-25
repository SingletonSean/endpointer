using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Core.Client.Http;
using Endpointer.Core.Client.Services.Refresh;
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

            services.AddRefitClient<IAPIRegisterService>(options.RefitSettings, endpointsConfiguration.RegisterEndpoint);
            services.AddRefitClient<IAPILoginService>(options.RefitSettings, endpointsConfiguration.LoginEndpoint);
            services.AddRefitClient<IAPIRefreshService>(options.RefitSettings, endpointsConfiguration.RefreshEndpoint);
            services.AddRefitClient<IAPILogoutService>(options.RefitSettings, endpointsConfiguration.LogoutEndpoint);

            if(options.AutoTokenRefresh)
            {
                services.AddAutoRefreshRefitClient<IAPILogoutEverywhereService>(options.RefitSettings, 
                    endpointsConfiguration.LogoutEndpoint, 
                    options.GetAutoRefreshTokenStore);
            }
            else
            {
                services.AddAccessTokenRefitClient<IAPILogoutService>(options.RefitSettings,
                    endpointsConfiguration.LogoutEndpoint,
                    getTokenStore);
            }

            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<ILogoutEverywhereService, LogoutEverywhereService>();
            services.AddSingleton<ILogoutService, LogoutService>();
            services.AddSingleton<IRefreshService, RefreshService>();

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
