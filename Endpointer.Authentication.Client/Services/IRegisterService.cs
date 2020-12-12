using Endpointer.Authentication.Client.Models.Responses;
using Endpointer.Authentication.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface IRegisterService
    {
        Task<Response> Register(RegisterRequest request);
    }
}
