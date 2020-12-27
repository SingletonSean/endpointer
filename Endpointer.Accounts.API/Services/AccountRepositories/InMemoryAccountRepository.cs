using Endpointer.Accounts.API.Services;
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

        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            _accounts.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_accounts.FirstOrDefault(u => u.Email == email));
        }

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_accounts.FirstOrDefault(u => u.Id == userId));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_accounts.FirstOrDefault(u => u.Username == username));
        }
    }
}
