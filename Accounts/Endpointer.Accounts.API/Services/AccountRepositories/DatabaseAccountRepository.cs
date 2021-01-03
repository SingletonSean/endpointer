using Endpointer.Accounts.API.Contexts;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Services.AccountRepositories
{
    public class DatabaseAccountRepository<TDbContext> : IAccountRepository
        where TDbContext : DbContext, IAccountsDbContext<User>
    {
        private readonly TDbContext _context;

        public DatabaseAccountRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(Guid userId)
        {
            return await _context.Accounts.FindAsync(userId);
        }
    }
}
