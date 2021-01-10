using Endpointer.Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    /// <summary>
    /// DbContext for the authentication API.
    /// </summary>
    /// <typeparam name="TUser">The type of user stored in the DbContext.</typeparam>
    public interface IAuthenticationDbContext<TUser> where TUser : class
    {
        /// <summary>
        /// Users stored in the database.
        /// </summary>
        DbSet<TUser> Users { get; }

        /// <summary>
        /// Refresh tokens stored in the database.
        /// </summary>
        DbSet<RefreshToken> RefreshTokens { get; }
    }
}