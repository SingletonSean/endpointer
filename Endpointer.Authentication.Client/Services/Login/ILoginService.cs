using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Endpointer.Authentication.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Login
{
    public interface ILoginService
    {
        /// <summary>
        /// Login a user by getting tokens.
        /// </summary>
        /// <param name="request">The request containing the login information.</param>
        /// <returns>The response with the user's tokens.</returns>
        /// <exception cref="UnauthorizedException">Thrown if user has invalid username or password.</exception>
        /// <exception cref="ValidationException">Thrown if request has validation errors.</exception>
        /// <exception cref="Exception">Thrown if login fails.</exception>
        Task<AuthenticatedUserResponse> Login(LoginRequest request);
    }
}
