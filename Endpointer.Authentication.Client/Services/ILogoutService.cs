using Refit;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services
{
    public interface ILogoutService
    {
        [Delete("/")]
        Task Logout([Authorize] string token);
    }
}
