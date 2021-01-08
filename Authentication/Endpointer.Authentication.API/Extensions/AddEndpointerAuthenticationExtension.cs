﻿using AutoMapper;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Mappers;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Extensions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Endpointer.Authentication.API.Extensions
{
    public static class AddEndpointerAuthenticationExtension
    {
        /// <summary>
        /// Add Endpointer Authentication services.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="authenticationConfiguration">The configuration for authentication services.</param>
        /// <param name="validationParameters">The validation parameters for access tokens.</param>
        /// <param name="configureOptions">Function configure additional options.</param>
        /// <returns>The service collection with registered services.</returns>
        public static IServiceCollection AddEndpointerAuthentication(this IServiceCollection services, 
            AuthenticationConfiguration authenticationConfiguration,
            TokenValidationParameters validationParameters,
            Func<EndpointerAuthenticationOptionsBuilder, EndpointerAuthenticationOptionsBuilder> configureOptions = null)
        {
            EndpointerAuthenticationOptionsBuilder optionsBuilder = new EndpointerAuthenticationOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerAuthenticationOptions options = optionsBuilder.Build();

            if(options.UseDatabase)
            {
                options.AddDbContext?.Invoke(services);
                options.AddDbUserRepository?.Invoke(services);
                options.AddDbRefreshTokenRepository?.Invoke(services);
            }
            else
            {
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();
            }

            services.AddSingleton(authenticationConfiguration);

            services.AddAutoMapper(typeof(DomainResponseProfile));

            services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

            services.AddScoped<RegisterEndpointHandler>();
            services.AddScoped<LoginEndpointHandler>();
            services.AddScoped<RefreshEndpointHandler>();
            services.AddScoped<LogoutEndpointHandler>();
            services.AddScoped<LogoutEverywhereEndpointHandler>();

            services.AddEndpointerCore(validationParameters);

            return services;
        }
    }
}
