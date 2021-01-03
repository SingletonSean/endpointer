using Microsoft.EntityFrameworkCore;

namespace Endpointer.Accounts.API.Contexts
{
    public interface IAccountsDbContext<TAccount> where TAccount : class
    {
        DbSet<TAccount> Accounts { get; }
    }
}