using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public interface IAccountDbContext<TUser> where TUser : class
    {
        DbSet<TUser> Users { get; }
    }
}