using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Users.API.EndpointerHandlers
{
    public class GetUserEndpointerHandler
    {
        public async Task<IActionResult> HandleGetUser()
        {
            return new OkResult();
        }
    }
}
