using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    public class EndpointerAuthenticationOptionsBuilder
    {
        private Action<IServiceCollection> _addDataSourceServices;
        private bool _requireEmailVerification;

        public EndpointerAuthenticationOptionsBuilder()
        {
            _addDataSourceServices = services =>
            {
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();
            };
            _requireEmailVerification = false;
        }

        /// <summary>
        /// Add database services to Endpointer with a DefaultAuthenticationDbContext.
        /// </summary>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithEntityFrameworkDataSource(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithEntityFrameworkDataSource<DefaultAuthenticationDbContext>(dbOptions);
        }

        /// <summary>
        /// Add Entity Framework database services to Endpointer.
        /// </summary>
        /// <typeparam name="TDbContext">The type of DbContext for the database.</typeparam>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithEntityFrameworkDataSource<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IAuthenticationDbContext<User>
        {
            dbOptions = dbOptions ?? (o => { });

            _addDataSourceServices = services =>
            {
                services.AddDbContext<TDbContext>(dbOptions);
                services.AddScoped<IUserRepository, DatabaseUserRepository<TDbContext>>();
                services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository<TDbContext>>();
            };

            return this;
        }

        /// <summary>
        /// Add data source services using a custom data source.
        /// </summary>
        /// <param name="addDataSourceServices">The callback to add custom data source services.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithCustomDataSource(Action<IServiceCollection> addDataSourceServices)
        {
            _addDataSourceServices = addDataSourceServices;

            return this;
        }

        /// <summary>
        /// Require new user's to verify their emails.
        /// </summary>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder RequireEmailVerification()
        {
            _requireEmailVerification = true;

            return this;
        }

        /// <summary>
        /// Build the Endpointer options.
        /// </summary>
        /// <returns>The built options.</returns>
        public EndpointerAuthenticationOptions Build()
        {
            return new EndpointerAuthenticationOptions()
            {
                RequireEmailVerification = _requireEmailVerification,
                AddDataSourceServices = _addDataSourceServices,
            };
        }
    }
}
