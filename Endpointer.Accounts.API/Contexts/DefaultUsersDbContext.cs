using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Users.API.Contexts
{
    public class DefaultUsersDbContext : DbContext, IUsersDbContext<User>
    {
        public DefaultUsersDbContext(DbContextOptions<DefaultUsersDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
