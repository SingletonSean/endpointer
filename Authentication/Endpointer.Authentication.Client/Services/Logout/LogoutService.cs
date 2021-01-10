using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Client.Services.Logout
{
    public class LogoutService : ILogoutService
    {
        private readonly IAPILogoutService _api;
        private readonly ILogger<LogoutService> _logger;

        public LogoutService(IAPILogoutService api, ILogger<LogoutService> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Logout(string refreshToken)
        {
            _logger.LogInformation("Sending logout request.");
            await _api.Logout(refreshToken);
            _logger.LogInformation("Successfully logged out.");
        }
    }
}
