using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Accounts.API.EndpointerHandlers
{
    public class GetAccountEndpointerHandler
    {
        private readonly HttpRequestAuthenticator _authenticator;

        public GetAccountEndpointerHandler(HttpRequestAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        public async Task<IActionResult> HandleGetAccount(HttpRequest request)
        {
            User user = await _authenticator.Authenticate(request);
            if(user == null)
            {
                return new UnauthorizedResult();
            }

            // TODO: Setup automapper.
            AccountResponse account = new AccountResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };

            return new OkObjectResult(new SuccessResponse<AccountResponse>(account));
        }
    }
}
