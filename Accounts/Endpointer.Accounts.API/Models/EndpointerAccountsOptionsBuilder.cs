﻿using Endpointer.Core.API.Models;
using Endpointer.Accounts.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Endpointer.Accounts.API.Services.AccountRepositories;

namespace Endpointer.Accounts.API.Models
{
    public class EndpointerAccountsOptionsBuilder
    {
        private bool _useDatabase;
        private Action<IServiceCollection> _addDbContext;
        private Action<IServiceCollection> _addDbAccountRepository;

        public EndpointerAccountsOptionsBuilder WithDatabase(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithDatabase<DefaultAccountDbContext>(dbOptions);
        }

        public EndpointerAccountsOptionsBuilder WithDatabase<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IAccountsDbContext<User>
        {
            _useDatabase = true;

            _addDbContext = services => services.AddDbContext<TDbContext>(dbOptions);
            _addDbAccountRepository = services => services.AddScoped<IAccountRepository, DatabaseAccountRepository<TDbContext>>();

            return this;
        }

        public EndpointerAccountsOptions Build()
        {
            return new EndpointerAccountsOptions()
            {
                UseDatabase = _useDatabase,
                AddDbContext = _addDbContext,
                AddDbAccountRepository = _addDbAccountRepository
            };
        }
    }
}