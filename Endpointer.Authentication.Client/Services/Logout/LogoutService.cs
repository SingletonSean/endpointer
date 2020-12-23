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
        public async Task Logout()
        {
            try
            {
                await _api.Logout();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message, ex.InnerException);
                }

                throw;
            }
        }
    }
}
