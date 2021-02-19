using Endpointer.Accounts.API.EndpointHandlers;
using Endpointer.Accounts.API.Models;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Core.API.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Endpointer.Accounts.API.Extensions
{
    public static class AddEndpointerAccountsExtension
    {
        /// <summary>
        /// Add Endpointer Accounts services.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="validationParameters">The validation parameters for access tokens.</param>
        /// <param name="configureOptions">Function to configure additional options.</param>
        /// <returns>The service collection with registered services.</returns>
        public static IServiceCollection AddEndpointerAccounts(this IServiceCollection services, 
            TokenValidationParameters validationParameters,
            Func<EndpointerAccountsOptionsBuilder, EndpointerAccountsOptionsBuilder> configureOptions = null)
        {
            EndpointerAccountsOptionsBuilder optionsBuilder = new EndpointerAccountsOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerAccountsOptions options = optionsBuilder.Build();

            options.AddDataSourceServices?.Invoke(services);

            services.AddScoped<GetAccountEndpointHandler>();

            services.AddEndpointerCore(validationParameters);

            return services;
        }
    }
}
