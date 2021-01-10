using Endpointer.Accounts.Client.Exceptions;
using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Accounts.Client.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAPIAccountService _api;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAPIAccountService api, ILogger<AccountService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<AccountResponse> GetAccount()
        {
            try
            {
                _logger.LogInformation("Sending get account request.");
                SuccessResponse<AccountResponse> response = await _api.GetAccount();

                if (response == null || response.Data == null)
                {
                    _logger.LogError("Response does not contain data.");
                    throw new Exception("Failed to deserialize API response.");
                }

                _logger.LogInformation("Successfully retrieved account.");
                return response.Data;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Get account request failed.");
                if(ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Authentication failed.");
                    throw new UnauthorizedException();
                }
                
                if(ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError("Account not found.");
                    throw new AccountNotFoundException();
                }

                throw;
            }
        }
    }
}
