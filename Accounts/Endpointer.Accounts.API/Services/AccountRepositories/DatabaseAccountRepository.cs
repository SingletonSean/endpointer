using Endpointer.Accounts.API.Contexts;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Services.AccountRepositories
{
    public class DatabaseAccountRepository<TDbContext> : IAccountRepository
        where TDbContext : DbContext, IAccountsDbContext<User>
    {
        private readonly TDbContext _context;
        private readonly ILogger<DatabaseAccountRepository<TDbContext>> _logger;

        public DatabaseAccountRepository(TDbContext context, ILogger<DatabaseAccountRepository<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid userId)
        {
            _logger.LogInformation("Finding account by id.");
            User user = await _context.Accounts.FindAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("Account not found.");
            }
            else
            {
                _logger.LogInformation("Successfully found account by id.");
            }

            return user;
        }
    }
}
