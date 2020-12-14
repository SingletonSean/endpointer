using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Demos.WPF.Services;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Containers
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

            services.AddEndpointerAuthenticationClient(endpointConfiguration, s => s.GetRequiredService<TokenStore>());

            services.AddSingleton<RenavigationService<RegisterViewModel>>();
            services.AddSingleton<RenavigationService<LoginViewModel>>();

            return services;
        }
    }
}
