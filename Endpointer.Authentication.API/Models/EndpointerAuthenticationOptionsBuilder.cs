using Endpointer.Authentication.API.Contexts;
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

        public EndpointerAuthenticationOptionsBuilder WithDatabase(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithDatabase<DefaultAuthenticationDbContext, User>(dbOptions);
        }

        public EndpointerAuthenticationOptionsBuilder WithDatabase<TDbContext, TUser>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : AuthenticationDbContext<TUser>
            where TUser : class
        {
            _useDatabase = true;
            _addDbContext = services => services.AddDbContext<TDbContext>(dbOptions);

            return this;
        }

        public EndpointerAuthenticationOptions Build()
        {
            return new EndpointerAuthenticationOptions()
            {
                UseDatabase = _useDatabase,
                AddDbContext = _addDbContext
            };
        }
    }
}
