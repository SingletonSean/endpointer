using Endpointer.Authentication.Client.Exceptions;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public class LogoutService : ILogoutService
    {
        private readonly IAPILogoutService _api;

        public LogoutService(IAPILogoutService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task Logout(string refreshToken)
        {
            await _api.Logout(refreshToken);
        }
    }
}
