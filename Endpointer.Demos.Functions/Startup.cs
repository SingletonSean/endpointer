using System;
using System.Text;
using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.API.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(Endpointer.Demos.Functions.Startup))]

namespace Endpointer.Demos.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration()
            {
                AccessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret"),
                AccessTokenExpirationMinutes = double.Parse(Environment.GetEnvironmentVariable("AccessTokenExpirationMinutes")),
                RefreshTokenSecret = Environment.GetEnvironmentVariable("RefreshTokenSecret"),
                RefreshTokenExpirationMinutes = double.Parse(Environment.GetEnvironmentVariable("RefreshTokenExpirationMinutes")),
                Audience = Environment.GetEnvironmentVariable("Audience"),
                Issuer = Environment.GetEnvironmentVariable("Issuer")
            };

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true
            };

            services.AddEndpointerAuthentication(authenticationConfiguration,
                tokenValidationParameters);
        }
    }
}
