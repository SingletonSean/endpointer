using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface IAuthenticationService
    {
        Task<Response<AuthenticatedUserResponse>> Login(LoginRequest request);
    }
}
