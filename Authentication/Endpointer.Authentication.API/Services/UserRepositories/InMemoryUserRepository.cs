using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        /// <inheritdoc />
        public Task Create(User user)
        {
            user.Id = Guid.NewGuid();

            _users.Add(user);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        /// <inheritdoc />
        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == userId));
        }

        /// <inheritdoc />
        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }
    }
}
