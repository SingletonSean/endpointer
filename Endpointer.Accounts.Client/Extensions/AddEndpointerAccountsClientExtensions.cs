using Endpointer.Accounts.Client.Models;
using Endpointer.Accounts.Client.Services.Accounts;
using Endpointer.Core.Client.Extensions;
using Endpointer.Core.Client.Models;
using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Client.Stores;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Endpointer.Accounts.Client.Extensions
{
    public static class AddEndpointerAccountsClientExtensions
    {
        public static IServiceCollection AddEndpointerAccountsClient(this IServiceCollection services, 
            AccountEndpointsConfiguration endpointsConfiguration,
            Func<IServiceProvider, IAccessTokenStore> getTokenStore,
            Func<EndpointerClientOptionsBuilder, EndpointerClientOptionsBuilder> configureOptions = null)
        {
            EndpointerClientOptionsBuilder optionsBuilder = new EndpointerClientOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerClientOptions options = optionsBuilder.Build();

            services.AddRefitClient<IAPIRefreshService>(options.RefitSettings, endpointsConfiguration.RefreshEndpoint);

            if(options.AutoTokenRefresh)
            {
                services.AddAutoRefreshRefitClient<IAPIAccountService>(options.RefitSettings, 
                    endpointsConfiguration.AccountEndpoint, 
                    options.GetAutoRefreshTokenStore);
            }
            else
            {
                services.AddAccessTokenRefitClient<IAPIAccountService>(options.RefitSettings,
                    endpointsConfiguration.AccountEndpoint,
                    getTokenStore);
            }
            
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IRefreshService, RefreshService>();

            return services;
        }
    }
}
