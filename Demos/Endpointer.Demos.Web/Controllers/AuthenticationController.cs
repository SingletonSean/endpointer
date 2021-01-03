﻿using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Demos.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly RegisterEndpointHandler _registerHandler;
        private readonly LoginEndpointHandler _loginHandler;
        private readonly RefreshEndpointHandler _refreshHandler;
        private readonly LogoutEndpointHandler _logoutHandler;
        private readonly LogoutEverywhereEndpointHandler _logoutEverywhereHandler;

        public AuthenticationController(RegisterEndpointHandler registerHandler,
            LoginEndpointHandler loginHandler, 
            RefreshEndpointHandler refreshHandler, 
            LogoutEndpointHandler logoutHandler,
            LogoutEverywhereEndpointHandler logoutEverywhereHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
            _refreshHandler = refreshHandler;
            _logoutHandler = logoutHandler;
            _logoutEverywhereHandler = logoutEverywhereHandler;
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

        [HttpDelete("logout/{refreshToken}")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            return await _logoutHandler.HandleLogout(refreshToken);
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> LogoutEverywhere()
        {
            return await _logoutEverywhereHandler.HandleLogoutEverywhere(HttpContext.Request);
        }
    }
}