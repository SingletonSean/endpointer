using Endpointer.Core.API.Http;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Endpointer.Core.API.Services.TokenDecoders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Endpointer.Core.API.Extensions
{
    public static class AddEndpointerCoreExtensions
    {
        /// <summary>
        /// Add Endpointer API core services.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="tokenValidationParameters">The required token validation parameters for services.</param>
        /// <returns>The service collection with the registered services.</returns>
        public static IServiceCollection AddEndpointerCore(this IServiceCollection services,
            TokenValidationParameters tokenValidationParameters)
        {
            services.AddSingleton(tokenValidationParameters);

            services.AddScoped<IHttpRequestAuthenticator, HttpRequestAuthenticator>();
            services.AddSingleton<ITokenClaimsDecoder, TokenHandlerTokenClaimsDecoder>();
            services.AddScoped<IAccessTokenDecoder, AccessTokenDecoder>();

            return services;
        }
    }
}
