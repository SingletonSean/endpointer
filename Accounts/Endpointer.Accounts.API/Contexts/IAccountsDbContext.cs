using Microsoft.EntityFrameworkCore;

namespace Endpointer.Accounts.API.Contexts
{
    /// <summary>
    /// DbContext for the account API.
    /// </summary>
    /// <typeparam name="TAccount">The type of account stored in the DbContext.</typeparam>
    public interface IAccountsDbContext<TAccount> where TAccount : class
    {
        /// <summary>
        /// Accounts stored in the database.
        /// </summary>
        DbSet<TAccount> Accounts { get; }
    }
}