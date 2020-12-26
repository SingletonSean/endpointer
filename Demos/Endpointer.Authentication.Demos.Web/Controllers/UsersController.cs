using Endpointer.Users.API.EndpointerHandlers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Demos.Web.Controllers
{
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly GetUserEndpointerHandler _getUserEndpointerHandler;

        public UsersController(GetUserEndpointerHandler getUserEndpointerHandler)
        {
            _getUserEndpointerHandler = getUserEndpointerHandler;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            return await _getUserEndpointerHandler.HandleGetUser();
        }
    }
}
