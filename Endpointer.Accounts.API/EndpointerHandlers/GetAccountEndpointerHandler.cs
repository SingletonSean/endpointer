using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.EndpointerHandlers
{
    public class GetAccountEndpointerHandler
    {
        private readonly IAccountRepository _accountRepository;
        private readonly HttpRequestAuthenticator _authenticator;

        public GetAccountEndpointerHandler(IAccountRepository accountRepository, HttpRequestAuthenticator authenticator)
        {
            _accountRepository = accountRepository;
            _authenticator = authenticator;
        }

        public async Task<IActionResult> HandleGetAccount(HttpRequest request)
        {
            User user = await _authenticator.Authenticate(request);
            if(user == null)
            {
                return new UnauthorizedResult();
            }

            User account = await _accountRepository.GetById(user.Id);

            // TODO: Setup automapper.
            AccountResponse accountResponse = new AccountResponse()
            {
                Id = account.Id,
                Email = account.Email,
                Username = account.Username
            };

            return new OkObjectResult(new SuccessResponse<AccountResponse>(accountResponse));
        }
    }
}
