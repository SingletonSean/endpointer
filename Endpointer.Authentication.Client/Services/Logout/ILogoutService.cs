using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public interface ILogoutService
    {
        /// <summary>
        /// Logout the current user by deleting the user's current refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to delete.</param>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task Logout(string refreshToken);
    }
}
