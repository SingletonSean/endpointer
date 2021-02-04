using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <inheritdoc />
        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            await _client
                .Child(_usersKey)
                .Child(user.Id.ToString())
                .PutAsync(user);

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetByEmail(string email)
        {
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderBy(nameof(User.Email))
                .EqualTo(email)
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Email != email)
            {
                return null;
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid userId)
        {
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderBy(nameof(User.Id))
                .EqualTo(userId.ToString())
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Id != userId)
            {
                return null;
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetByUsername(string username)
        {
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderBy(nameof(User.Username))
                .EqualTo(username)
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Username != username)
            {
                return null;
            }

            return user;
        }
    }
}
