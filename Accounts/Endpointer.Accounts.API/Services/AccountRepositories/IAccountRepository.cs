using Endpointer.Core.API.Models;
using System;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.Services.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<User> GetById(Guid id);
    }
}
