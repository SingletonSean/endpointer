using Microsoft.EntityFrameworkCore;

namespace Endpointer.Users.API.Contexts
{
    public interface IUsersDbContext<TUser> where TUser : class
    {
        DbSet<TUser> Users { get; }
    }
}