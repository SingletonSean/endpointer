using System;
using Endpointer.Authentication.Client.Exceptions;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public interface ILogoutService
    {
        /// <summary>
        /// Logout the current user.
        /// </summary>
        /// <exception cref="UnauthorizedException">Thrown if user has invalid access token.</exception>
        /// <exception cref="Exception">Thrown if logout fails.</exception>
        Task Logout();
    }
}
