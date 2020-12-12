using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Endpointer.Authentication.Client.Extensions
{
    public static class AddEndpointerAuthenticationClientExtensions
    {
        public static IServiceCollection AddEndpointerAuthenticationClient(this IServiceCollection services, 
            AuthenticationEndpointsConfiguration endpointConfiguration)
        {
            RefitSettings refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer()); 

            services.AddRefitClient<IRegisterService>(refitSettings).ConfigureHttpClient(o =>
            {
                o.BaseAddress = new Uri(endpointConfiguration.RegisterEndpoint);
            });

            services.AddRefitClient<ILoginService>(refitSettings).ConfigureHttpClient(o =>
            {
                o.BaseAddress = new Uri(endpointConfiguration.LoginEndpoint);
            });

            services.AddRefitClient<IRefreshService>(refitSettings).ConfigureHttpClient(o =>
            {
                o.BaseAddress = new Uri(endpointConfiguration.RefreshEndpoint);
            });

            services.AddRefitClient<ILogoutService>(refitSettings).ConfigureHttpClient(o =>
            {
                o.BaseAddress = new Uri(endpointConfiguration.LogoutEndpoint);
            });

            return services;
        }
    }
}
