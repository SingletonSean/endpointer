using Endpointer.Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public class AuthenticationDbContext<TUser> : DbContext where TUser : class
    {
        public AuthenticationDbContext(DbContextOptions<DefaultAuthenticationDbContext> options) : base(options) { }

        public DbSet<TUser> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
