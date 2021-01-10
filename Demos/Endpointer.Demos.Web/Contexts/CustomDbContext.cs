using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Endpointer.Accounts.API.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Demos.Web.Contexts
{
    public class CustomDbContext : DbContext, IAuthenticationDbContext<User>, IAccountsDbContext<User>
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Accounts => Users;
    }
}
