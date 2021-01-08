using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LogoutEndpointHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutEndpointHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Handle a logout by deleting a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to logout.</param>
        /// <returns>The result of the logout.</returns>
        public async Task<IActionResult> HandleLogout(string refreshToken)
        {
            await _refreshTokenRepository.DeleteByToken(refreshToken);

            return new NoContentResult();
        }
    }
}
