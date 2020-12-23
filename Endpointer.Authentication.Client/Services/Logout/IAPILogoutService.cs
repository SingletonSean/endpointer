using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public interface IAPILogoutService
    {
        /// <summary>
        /// Logout the current user.
        /// </summary>
        /// <exception cref="ApiException">Thrown if request fails.</returns>
        [Delete("/")]
        Task Logout();
    }
}
