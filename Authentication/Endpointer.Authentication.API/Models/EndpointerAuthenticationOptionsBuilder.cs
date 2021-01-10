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
        private bool _useDatabase;
        private Action<IServiceCollection> _addDbContext;
        private Action<IServiceCollection> _addDbUserRepository;
        private Action<IServiceCollection> _addDbRefreshTokenRepository;

        /// <summary>
        /// Add database services to Endpointer with a DefaultAuthenticationDbContext.
        /// </summary>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithDatabase(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithDatabase<DefaultAuthenticationDbContext>(dbOptions);
        }

        /// <summary>
        /// Add database services to Endpointer.
        /// </summary>
        /// <typeparam name="TDbContext">The type of DbContext for the database.</typeparam>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithDatabase<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IAuthenticationDbContext<User>
        {
            _useDatabase = true;

            _addDbContext = services => services.AddDbContext<TDbContext>(dbOptions);
            _addDbUserRepository = services => services.AddScoped<IUserRepository, DatabaseUserRepository<TDbContext>>();
            _addDbRefreshTokenRepository = services => services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository<TDbContext>>();

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
                UseDatabase = _useDatabase,
                AddDbContext = _addDbContext,
                AddDbUserRepository = _addDbUserRepository,
                AddDbRefreshTokenRepository = _addDbRefreshTokenRepository
            };
        }
    }
}
