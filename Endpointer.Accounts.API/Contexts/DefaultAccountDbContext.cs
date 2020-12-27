using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Accounts.API.Contexts
{
    public class DefaultAccountDbContext : DbContext, IAccountsDbContext<User>
    {
        public DefaultAccountDbContext(DbContextOptions<DefaultAccountDbContext> options) : base(options) { }

        public DbSet<User> Accounts { get; set; }
    }
}
