using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Services.AccountRepositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<User> _accounts = new List<User>();

        /// <inheritdoc />
        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_accounts.FirstOrDefault(u => u.Id == userId));
        }
    }
}
