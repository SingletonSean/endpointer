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
using System;
using Microsoft.Extensions.Logging;

namespace Endpointer.Accounts.API.EndpointHandlers
{
    public class GetAccountEndpointHandler
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpRequestAuthenticator _authenticator;
        private readonly ILogger<GetAccountEndpointHandler> _logger;

        public GetAccountEndpointHandler(IAccountRepository accountRepository,
            IHttpRequestAuthenticator authenticator, 
            ILogger<GetAccountEndpointHandler> logger)
        {
            _accountRepository = accountRepository;
            _authenticator = authenticator;
            _logger = logger;
        }

        /// <summary>
        /// Handle a get account request for the request user.
        /// </summary>
        /// <param name="request">The request with a user to authenticate.</param>
        /// <returns>The result of the request.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        public async Task<IActionResult> HandleGetAccount(HttpRequest request)
        {
            try
            {
                _logger.LogInformation("Authenticating request.");
                User user = await _authenticator.Authenticate(request);

                _logger.LogInformation("Finding account for user id {UserId}.", user.Id);
                User account = await _accountRepository.GetById(user.Id);

                if (account == null)
                {
                    _logger.LogError("Account not found for user id {UserId}.", user.Id);
                    return new NotFoundResult();
                }

                AccountResponse accountResponse = new AccountResponse()
                {
                    Id = account.Id,
                    Email = account.Email,
                    Username = account.Username,
                    IsEmailVerified = account.IsEmailVerified
                };

                _logger.LogInformation("Successfully retrieved account for user id {UserId}.", user.Id);
                return new OkObjectResult(new SuccessResponse<AccountResponse>(accountResponse));
            }
            catch (BearerSchemeNotProvidedException ex)
            {
                _logger.LogError(ex, "Bearer scheme not provided.");
                return new UnauthorizedResult();
            }
            catch (SecurityTokenDecryptionFailedException ex)
            {
                _logger.LogError(ex, "Failed to authenticate request.");
                return new UnauthorizedResult();
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Failed to authenticate request.");
                return new UnauthorizedResult();
            }
        }
    }
}
