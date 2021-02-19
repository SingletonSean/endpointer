using Endpointer.Core.API.Models;
using Endpointer.Accounts.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Endpointer.Accounts.API.Services.AccountRepositories;

namespace Endpointer.Accounts.API.Models
{
    public class EndpointerAccountsOptionsBuilder
    {
        private Action<IServiceCollection> _addDataSourceServices;

        public EndpointerAccountsOptionsBuilder()
        {
            _addDataSourceServices = (s) =>
            {
                s.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
            };
        }

        /// <summary>
        /// Add database services to Endpointer with a DefaultAccountDbContext.
        /// </summary>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAccountsOptionsBuilder WithEntityFrameworkDataSource(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithEntityFrameworkDataSource<DefaultAccountDbContext>(dbOptions);
        }

        /// <summary>
        /// Add database services to Endpointer.
        /// </summary>
        /// <typeparam name="TDbContext">The type of DbContext for the database.</typeparam>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAccountsOptionsBuilder WithEntityFrameworkDataSource<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IAccountsDbContext<User>
        {
            dbOptions = dbOptions ?? (o => { });

            _addDataSourceServices = (s) =>
            {
                s.AddDbContext<TDbContext>(dbOptions);
                s.AddScoped<IAccountRepository, DatabaseAccountRepository<TDbContext>>();
            };

            return this;
        }

        /// <summary>
        /// Add data source services using a custom data source.
        /// </summary>
        /// <param name="addDataSourceServices">The callback to add custom data source services.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAccountsOptionsBuilder WithCustomDataSource(Action<IServiceCollection> addDataSourceServices)
        {
            _addDataSourceServices = addDataSourceServices;

            return this;
        }

        /// <summary>
        /// Build the Endpointer options.
        /// </summary>
        /// <returns>The built options.</returns>
        public EndpointerAccountsOptions Build()
        {
            return new EndpointerAccountsOptions()
            {
                AddDataSourceServices = _addDataSourceServices
            };
        }
    }
}
