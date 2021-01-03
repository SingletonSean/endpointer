using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Threading.Tasks;

namespace Endpointer.Accounts.Client.Services.Accounts
{
    public interface IAPIAccountService
    {
        /// <summary>
        /// Get the current user's account.
        /// </summary>
        /// <returns>The user's account information.</returns>
        /// <exception cref="ApiException">Thrown if getting account fails.</exception>
        [Get("")]
        Task<SuccessResponse<AccountResponse>> GetAccount();
    }
}
