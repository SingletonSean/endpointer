using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FirebaseUserRepository> _logger;

        public FirebaseUserRepository(FirebaseClient client, ILogger<FirebaseUserRepository> logger)
        {
            _client = client;
            _logger = logger;

            _usersKey = DEFAULT_USERS_KEY;
        }

        /// <inheritdoc />
        public async Task Create(User user)
        {
            user.Id = Guid.NewGuid();

            _logger.LogInformation("Creating user.");
            await _client
                .Child(_usersKey)
                .Child(user.Id.ToString())
                .PutAsync(user);

            _logger.LogInformation("Successfully created user.");
        }

        /// <inheritdoc />
        public async Task<User> GetByEmail(string email)
        {
            _logger.LogInformation("Querying user {UserId}.", email);
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderBy(nameof(User.Email))
                .EqualTo(email)
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Email != email)
            {
                _logger.LogWarning("User not found.");
                return null;
            }

            _logger.LogInformation("Successfully found user.");

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid userId)
        {
            _logger.LogInformation("Querying user {UserId}.", userId);
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderByKey()
                .EqualTo(userId.ToString())
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Id != userId)
            {
                _logger.LogWarning("User not found.");
                return null;
            }

            _logger.LogInformation("Successfully found user.");

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetByUsername(string username)
        {
            _logger.LogInformation("Querying user {Username}.", username);
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_usersKey)
                .OrderBy(nameof(User.Username))
                .EqualTo(username)
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Username != username)
            {
                _logger.LogWarning("User not found.");
                return null;
            }

            _logger.LogInformation("Successfully found user.");

            return user;
        }

        /// <inheritdoc />
        public async Task Update(Guid id, Action<User> update)
        {
            User user = await GetById(id);

            if(user == null)
            {
                _logger.LogError("User not found.");
                throw new Exception("User not found.");
            }

            update(user);

            _logger.LogInformation("Updating user {UserId}.", id);
            await _client
                .Child(_usersKey)
                .Child(user.Id.ToString())
                .PutAsync(user);

            _logger.LogInformation("Successfully updated user.");
        }
    }
}
