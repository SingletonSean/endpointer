using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    /// <summary>
    /// Fallback DbContext for an authentication API.
    /// </summary>
    public class DefaultAuthenticationDbContext : DbContext, IAuthenticationDbContext<User>
    {
        public DefaultAuthenticationDbContext(DbContextOptions<DefaultAuthenticationDbContext> options) : base(options) { }

        /// <inheritdoc />
        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
