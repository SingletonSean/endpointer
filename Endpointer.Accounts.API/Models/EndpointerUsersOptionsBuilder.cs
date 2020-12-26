using Endpointer.Core.API.Models;
using Endpointer.Users.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Users.API.Models
{
    public class EndpointerUsersOptionsBuilder
    {
        private bool _useDatabase;
        private Action<IServiceCollection> _addDbContext;

        public EndpointerUsersOptionsBuilder WithDatabase(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithDatabase<DefaultUsersDbContext>(dbOptions);
        }

        public EndpointerUsersOptionsBuilder WithDatabase<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IUsersDbContext<User>
        {
            _useDatabase = true;

            _addDbContext = services => services.AddDbContext<TDbContext>(dbOptions);

            return this;
        }

        public EndpointerUsersOptions Build()
        {
            return new EndpointerUsersOptions()
            {
                UseDatabase = _useDatabase,
                AddDbContext = _addDbContext
            };
        }
    }
}
