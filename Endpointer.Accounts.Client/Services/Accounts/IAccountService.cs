using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace Endpointer.Accounts.Client.Services.Accounts
{
    public interface IAccountService
    {
        /// <summary>
        /// Get the current user's account.
        /// </summary>
        /// <returns>The user's account information.</returns>
        /// <exception cref="UnauthorizedException">Thrown if user has invalid access token.</exception>
        /// <exception cref="Exception">Thrown if getting account fails.</exception>
        Task<AccountResponse> GetAccount();
    }
}
