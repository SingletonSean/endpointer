using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LogoutEndpointHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<LogoutEndpointHandler> _logger;

        public LogoutEndpointHandler(IRefreshTokenRepository refreshTokenRepository, ILogger<LogoutEndpointHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handle a logout by deleting a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to logout.</param>
        /// <returns>The result of the logout.</returns>
        public async Task<IActionResult> HandleLogout(string refreshToken)
        {
            _logger.LogInformation("Logging out user.");
            await _refreshTokenRepository.DeleteByToken(refreshToken);

            _logger.LogInformation("Successfully logged out user.");
            return new NoContentResult();
        }
    }
}
