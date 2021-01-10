using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Accounts.API.Contexts
{
    /// <summary>
    /// Fallback DbContext for an account API.
    /// </summary>
    public class DefaultAccountDbContext : DbContext, IAccountsDbContext<User>
    {
        public DefaultAccountDbContext(DbContextOptions options) : base(options) { }
        public DefaultAccountDbContext(DbContextOptions<DefaultAccountDbContext> options) : base(options) { }

        /// <inheritdoc />
        public DbSet<User> Accounts { get; set; }
    }
}
