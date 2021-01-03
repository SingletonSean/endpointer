using Endpointer.Accounts.API.EndpointerHandlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Demos.Web.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly GetAccountEndpointerHandler _getAccountEndpointerHandler;

        public AccountController(GetAccountEndpointerHandler getAccountEndpointerHandler)
        {
            _getAccountEndpointerHandler = getAccountEndpointerHandler;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAccount()
        {
            return await _getAccountEndpointerHandler.HandleGetAccount(HttpContext.Request);
        }
    }
}
