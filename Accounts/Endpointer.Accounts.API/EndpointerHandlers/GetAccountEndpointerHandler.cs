using Endpointer.Accounts.API.Services.AccountRepositories;
using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            try
            {
                User user = await _authenticator.Authenticate(request);

                User account = await _accountRepository.GetById(user.Id);
                if (account == null)
                {
                    return new NotFoundResult();
                }

                // TODO: Setup automapper.
                AccountResponse accountResponse = new AccountResponse()
                {
                    Id = account.Id,
                    Email = account.Email,
                    Username = account.Username
                };

                return new OkObjectResult(new SuccessResponse<AccountResponse>(accountResponse));
            }
            catch (BearerSchemeNotProvidedException)
            {
                return new UnauthorizedResult();
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return new UnauthorizedResult();
            }
            catch (SecurityTokenException)
            {
                return new UnauthorizedResult();
            }
        }
    }
}
