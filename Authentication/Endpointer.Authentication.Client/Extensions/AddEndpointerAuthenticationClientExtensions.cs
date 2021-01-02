using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Core.Client.Extensions;
using Endpointer.Core.Client.Models;
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
            Func<EndpointerClientOptionsBuilder, EndpointerClientOptionsBuilder> configureOptions = null)
        {
            EndpointerClientOptionsBuilder optionsBuilder = new EndpointerClientOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerClientOptions options = optionsBuilder.Build();

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
    }
}
