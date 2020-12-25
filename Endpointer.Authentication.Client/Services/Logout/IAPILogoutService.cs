using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public interface IAPILogoutService
    {
        /// <summary>
        /// Logout the current user by deleting the user's current refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to delete.</param>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Delete("/{refreshToken}")]
        Task Logout(string refreshToken);
    }
}
