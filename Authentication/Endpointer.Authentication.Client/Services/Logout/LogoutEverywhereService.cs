using Endpointer.Core.Client.Exceptions;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public class LogoutEverywhereService : ILogoutEverywhereService
    {
        private readonly IAPILogoutEverywhereService _api;

        public LogoutEverywhereService(IAPILogoutEverywhereService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task LogoutEverywhere()
        {
            try
            {
                await _api.LogoutEverywhere();
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
