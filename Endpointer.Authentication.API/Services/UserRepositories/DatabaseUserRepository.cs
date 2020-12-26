using Endpointer.Authentication.API.Contexts;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRepositories
{
    public class DatabaseUserRepository<TDbContext> : IUserRepository
        where TDbContext : DbContext, IAuthenticationDbContext<User>
    {
        private readonly TDbContext _context;

        public DatabaseUserRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
