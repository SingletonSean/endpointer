using Endpointer.Accounts.API.EndpointHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Demos.Web.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly GetAccountEndpointHandler _getAccountEndpointHandler;

        public AccountController(GetAccountEndpointHandler getAccountEndpointHandler)
        {
            _getAccountEndpointHandler = getAccountEndpointHandler;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAccount()
        {
            return await _getAccountEndpointHandler.HandleGetAccount(HttpContext.Request);
        }
    }
}
