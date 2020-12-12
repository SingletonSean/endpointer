using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Demos.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly RegisterEndpointHandler _registerHandler;
        private readonly LoginEndpointHandler _loginHandler;
        private readonly RefreshEndpointHandler _refreshHandler;
        private readonly LogoutEndpointHandler _logoutHandler;

        public AuthenticationController(RegisterEndpointHandler registerHandler,
            LoginEndpointHandler loginHandler, 
            RefreshEndpointHandler refreshHandler, 
            LogoutEndpointHandler logoutHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
            _refreshHandler = refreshHandler;
            _logoutHandler = logoutHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            return await _registerHandler.HandleRegister(registerRequest, ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return await _loginHandler.HandleLogin(loginRequest, ModelState);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            return await _refreshHandler.HandleRefresh(refreshRequest, ModelState);
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            return await _logoutHandler.HandleLogout(HttpContext.Request);
        }
    }
}
