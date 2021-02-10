using AutoMapper;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Mappers;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications;
using Endpointer.Authentication.API.Services.UserRegisters;
using Endpointer.Core.API.Extensions;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
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
        /// <param name="configureOptions">Function to configure additional options.</param>
        /// <returns>The service collection with registered services.</returns>
        public static IServiceCollection AddEndpointerAuthentication(this IServiceCollection services, 
            AuthenticationConfiguration authenticationConfiguration,
            TokenValidationParameters validationParameters,
            Func<EndpointerAuthenticationOptionsBuilder, EndpointerAuthenticationOptionsBuilder> configureOptions = null)
        {
            EndpointerAuthenticationOptionsBuilder optionsBuilder = new EndpointerAuthenticationOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerAuthenticationOptions options = optionsBuilder.Build();

            options.AddDataSourceServices(services);

            services.AddSingleton(authenticationConfiguration);

            services.AddAutoMapper(typeof(DomainResponseProfile));

            services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

            if(options.RequireEmailVerification)
            {
                FluentEmailServicesBuilder emailBuilder = services.AddFluentEmail(options.EmailVerificationConfiguration.EmailFromAddress);
                options.EmailVerificationConfiguration.ConfigureFluentEmailServices?.Invoke(emailBuilder);

                services.AddSingleton(options.EmailVerificationConfiguration);
                services.AddSingleton<IEmailVerificationTokenGenerator, EmailVerificationTokenGenerator>();
                services.AddSingleton<IEmailVerificationTokenValidator, EmailVerificationTokenValidator>();
                services.AddScoped<IEmailSender, FluentEmailSender>();
                services.AddScoped<IUserRegister, EmailVerificationUserRegister>();
            }
            else
            {
                services.AddScoped<IUserRegister, UserRegister>();
            }

            services.AddScoped<RegisterEndpointHandler>();
            services.AddScoped<LoginEndpointHandler>();
            services.AddScoped<RefreshEndpointHandler>();
            services.AddScoped<LogoutEndpointHandler>();
            services.AddScoped<LogoutEverywhereEndpointHandler>();
            services.AddScoped<VerifyEmailEndpointerHandler>();

            services.AddEndpointerCore(validationParameters);

            return services;
        }
    }
}
