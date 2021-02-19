using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Core.API.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Firebase.Services.AccountRepositories
{
    public class FirebaseAccountRepository : IAccountRepository
    {
        private const string DEFAULT_ACCOUNTS_KEY = "users";

        private readonly string _accountsKey;
        private readonly FirebaseClient _client;
        private readonly ILogger<FirebaseAccountRepository> _logger;

        public FirebaseAccountRepository(FirebaseClient client, ILogger<FirebaseAccountRepository> logger)
        {
            _client = client;
            _logger = logger;

            _accountsKey = DEFAULT_ACCOUNTS_KEY;
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid userId)
        {
            _logger.LogInformation("Querying account {UserId}.", userId);
            IReadOnlyCollection<FirebaseObject<User>> users = await _client
                .Child(_accountsKey)
                .OrderByKey()
                .EqualTo(userId.ToString())
                .OnceAsync<User>();

            User user = users.FirstOrDefault()?.Object;

            if (user == null || user.Id != userId)
            {
                _logger.LogWarning("Account not found.");
                return null;
            }

            _logger.LogInformation("Successfully found account.");

            return user;
        }
    }
}
