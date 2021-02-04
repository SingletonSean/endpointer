using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRepositories
{
    public class FirebaseUserRepository : IUserRepository
    {
        private const string DEFAULT_USERS_KEY = "users";

        private readonly string _usersKey;
        private readonly FirebaseClient _client;

        public FirebaseUserRepository(FirebaseClient client)
        {
            _client = client;
            
            _usersKey = DEFAULT_USERS_KEY;
        }

        public async Task<User> Create(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
