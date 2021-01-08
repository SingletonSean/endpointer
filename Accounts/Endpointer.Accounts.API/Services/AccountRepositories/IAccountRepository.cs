using Endpointer.Core.API.Models;
using System;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Services.AccountRepositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get an account by id.
        /// </summary>
        /// <param name="id">The id of the account to find.</param>
        /// <returns>The account with the id. Null if account not found.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<User> GetById(Guid id);
    }
}
