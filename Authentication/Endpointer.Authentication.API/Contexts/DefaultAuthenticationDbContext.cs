using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public class DefaultAuthenticationDbContext : DbContext, IAuthenticationDbContext<User>
    {
        public DefaultAuthenticationDbContext(DbContextOptions<DefaultAuthenticationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
