using Endpointer.Users.API.EndpointerHandlers;
using Endpointer.Users.API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Users.API.Extensions
{
    public static class AddEndpointerUsersExtension
    {
        public static IServiceCollection AddEndpointerUsers(this IServiceCollection services, 
            Func<EndpointerUsersOptionsBuilder, EndpointerUsersOptionsBuilder> configureOptions = null)
        {
            EndpointerUsersOptionsBuilder optionsBuilder = new EndpointerUsersOptionsBuilder();
            configureOptions?.Invoke(optionsBuilder);
            EndpointerUsersOptions options = optionsBuilder.Build();

            if(options.UseDatabase)
            {
                options.AddDbContext?.Invoke(services);
            }
            else
            {

            }

            services.AddScoped<GetUserEndpointerHandler>();

            return services;
        }
    }
}
