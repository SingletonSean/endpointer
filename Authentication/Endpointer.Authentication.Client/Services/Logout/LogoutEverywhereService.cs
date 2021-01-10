using Endpointer.Core.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public class LogoutEverywhereService : ILogoutEverywhereService
    {
        private readonly IAPILogoutEverywhereService _api;
        private readonly ILogger<LogoutEverywhereService> _logger;

        public LogoutEverywhereService(IAPILogoutEverywhereService api, ILogger<LogoutEverywhereService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task LogoutEverywhere()
        {
            try
            {
                _logger.LogInformation("Sending logout everywhere request.");
                await _api.LogoutEverywhere();
                _logger.LogInformation("Successfully logged out everywhere.");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Logout everywhere request failed.");
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized logout everywhere request.");
                    throw new UnauthorizedException(ex.Message, ex.InnerException);
                }

                _logger.LogError("Unknown error.");
                throw;
            }
        }
    }
}
