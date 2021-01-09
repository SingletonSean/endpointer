using Endpointer.Accounts.Client.Exceptions;
using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Refit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Accounts.Client.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAPIAccountService _api;

        public AccountService(IAPIAccountService api)
        {
            _api = api;
        }

        /// <inheritdoc />
        public async Task<AccountResponse> GetAccount()
        {
            try
            {
                SuccessResponse<AccountResponse> response = await _api.GetAccount();

                if (response == null || response.Data == null)
                {
                    throw new Exception("Failed to deserialize API response.");
                }

                return response.Data;
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException();
                }
                
                if(ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new AccountNotFoundException();
                }

                throw;
            }
        }
    }
}
