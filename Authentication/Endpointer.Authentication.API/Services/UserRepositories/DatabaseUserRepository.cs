using Endpointer.Authentication.API.Contexts;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRepositories
{
    public class DatabaseUserRepository<TDbContext> : IUserRepository
        where TDbContext : DbContext, IAuthenticationDbContext<User>
    {
        private readonly TDbContext _context;
        private readonly ILogger<DatabaseUserRepository<TDbContext>> _logger;

        public DatabaseUserRepository(TDbContext context, ILogger<DatabaseUserRepository<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Create(User user)
        {
            _logger.LogInformation("Saving new user.");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created new user.");
        }

        /// <inheritdoc />
        public async Task<User> GetByEmail(string email)
        {
            _logger.LogInformation("Finding user by email.");
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                _logger.LogWarning("User not found.");
            }
            else
            {
                _logger.LogInformation("Successfully found user by email.");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid userId)
        {
            _logger.LogInformation("Finding user by id.");
            User user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User not found.");
            }
            else
            {
                _logger.LogInformation("Successfully found user by id.");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task<User> GetByUsername(string username)
        {
            _logger.LogInformation("Finding user by username.");
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                _logger.LogWarning("User not found.");
            }
            else
            {
                _logger.LogInformation("Successfully found user by username.");
            }

            return user;
        }

        /// <inheritdoc />
        public async Task Update(Guid id, Action<User> update)
        {
            User user = new User() { Id = id };

            _context.Attach(user);
            update(user);

            await _context.SaveChangesAsync();
        }
    }
}
