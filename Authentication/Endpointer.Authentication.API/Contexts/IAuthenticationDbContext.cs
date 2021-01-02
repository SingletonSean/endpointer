using Endpointer.Authentication.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public interface IAuthenticationDbContext<TUser> where TUser : class
    {
        DbSet<TUser> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
    }
}