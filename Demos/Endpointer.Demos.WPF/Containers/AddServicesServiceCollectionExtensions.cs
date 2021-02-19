using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Client.Models;
using Endpointer.Accounts.Client.Extensions;
using Endpointer.Demos.WPF.Services;
using Endpointer.Demos.WPF.Stores;
using Endpointer.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Endpointer.Accounts.Client.Models;

namespace Endpointer.Demos.WPF.Containers
{
    public static class AddServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            string baseAddress = "https://localhost:5001/";
            AuthenticationEndpointsConfiguration authenticationEndpointConfiguration = new AuthenticationEndpointsConfiguration()
            {
                RegisterEndpoint = baseAddress + "register",
                LoginEndpoint = baseAddress + "login",
                RefreshEndpoint = baseAddress + "refresh",
                LogoutEndpoint = baseAddress + "logout",
                VerifyEmailEndpoint = baseAddress + "verify",
                SendVerifyEmailEndpoint = baseAddress + "send-verify",
            };
            AccountEndpointsConfiguration accountsEndpointConfiguration = new AccountEndpointsConfiguration()
            {
                AccountEndpoint = baseAddress + "account",
                RefreshEndpoint = baseAddress + "refresh"
            };

            services.AddEndpointerAuthenticationClient(authenticationEndpointConfiguration, 
                s => s.GetRequiredService<TokenStore>(),
                o => o.WithAutoTokenRefresh(s => s.GetRequiredService<TokenStore>()));

            services.AddEndpointerAccountsClient(accountsEndpointConfiguration,
                s => s.GetRequiredService<TokenStore>(),
                o => o.WithAutoTokenRefresh(s => s.GetRequiredService<TokenStore>()));

            services.AddSingleton<RenavigationService<RegisterViewModel>>();
            services.AddSingleton<RenavigationService<LoginViewModel>>();
            services.AddSingleton<RenavigationService<AccountViewModel>>();
            services.AddSingleton<RenavigationService<VerifyEmailViewModel>>();

            return services;
        }
    }
}
