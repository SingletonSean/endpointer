using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public interface IAPILogoutEverywhereService
    {
        /// <summary>
        /// Logout the current user everywhere.
        /// </summary>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Delete("/")]
        Task LogoutEverywhere();
    }
}
