using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using System.Threading.Tasks;
using System;

namespace Endpointer.Authentication.API.Services.Authenticators
{
    public interface IAuthenticator
    {
        /// <summary>
        /// Authenticate the user.
        /// </summary>
        /// <param name="user">The user to authenticate.</param>
        /// <returns>The tokens for the user.</returns>
        /// <exception cref="Exception">Thrown if authentication fails.</exception>
        Task<AuthenticatedUser> Authenticate(User user);
    }
}