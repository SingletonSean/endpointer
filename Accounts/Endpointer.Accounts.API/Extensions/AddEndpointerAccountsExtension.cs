﻿using Endpointer.Accounts.API.EndpointerHandlers;
using Endpointer.Accounts.API.Models;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Accounts.API.Extensions
{
    public static class AddEndpointerAccountsExtension
    {
        public static IServiceCollection AddEndpointerAccounts(this IServiceCollection services, 
            Func<EndpointerAccountsOptionsBuilder, EndpointerAccountsOptionsBuilder> configureOptions = null)
        {
            EndpointerAccountsOptionsBuilder optionsBuilder = new EndpointerAccountsOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerAccountsOptions options = optionsBuilder.Build();

            if(options.UseDatabase)
            {
                options.AddDbContext?.Invoke(services);
                options.AddDbAccountRepository?.Invoke(services);
            }
            else
            {
                services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
            }

            services.AddScoped<HttpRequestAuthenticator>();
            services.AddSingleton<AccessTokenDecoder>();

            services.AddScoped<GetAccountEndpointerHandler>();

            return services;
        }
    }
}