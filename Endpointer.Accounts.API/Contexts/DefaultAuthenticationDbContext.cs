using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public class DefaultAccountDbContext : DbContext, IAccountDbContext<User>
    {
        public DefaultAccountDbContext(DbContextOptions<DefaultAccountDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
