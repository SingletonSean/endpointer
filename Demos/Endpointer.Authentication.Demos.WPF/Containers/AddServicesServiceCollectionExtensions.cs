using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Client.Models;
using Endpointer.Demos.WPF.Services;
using Endpointer.Demos.WPF.Stores;
using Endpointer.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Demos.WPF.Containers
{
    public static class AddServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            string baseAddress = "https://localhost:5001/";
            AuthenticationEndpointsConfiguration endpointConfiguration = new AuthenticationEndpointsConfiguration()
            {
                RegisterEndpoint = baseAddress + "register",
                LoginEndpoint = baseAddress + "login",
                RefreshEndpoint = baseAddress + "refresh",
                LogoutEndpoint = baseAddress + "logout",
            };

            services.AddEndpointerAuthenticationClient(endpointConfiguration, 
                s => s.GetRequiredService<TokenStore>(),
                o => o.WithAutoTokenRefresh(s => s.GetRequiredService<TokenStore>()));

            services.AddSingleton<RenavigationService<RegisterViewModel>>();
            services.AddSingleton<RenavigationService<LoginViewModel>>();

            return services;
        }
    }
}
