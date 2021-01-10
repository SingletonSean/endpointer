using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LogoutEverywhereEndpointHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpRequestAuthenticator _requestAuthenticator;
        private readonly ILogger<LogoutEverywhereEndpointHandler> _logger;

        public LogoutEverywhereEndpointHandler(IRefreshTokenRepository refreshTokenRepository,
            IHttpRequestAuthenticator requestAuthenticator, ILogger<LogoutEverywhereEndpointHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _requestAuthenticator = requestAuthenticator;
            _logger = logger;
        }

        /// <summary>
        /// Handle a logout everywhere by deleting all refresh tokens.
        /// </summary>
        /// <param name="request">The request with the authenticated user.</param>
        /// <returns>The result of the logout everywhere.</returns>
        /// <exception cref="Exception">Thrown if logout everywhere fails.</exception>
        public async Task<IActionResult> HandleLogoutEverywhere(HttpRequest request)
        {
            try
            {
                _logger.LogInformation("Authenticating request.");
                User user = await _requestAuthenticator.Authenticate(request);

                _logger.LogInformation("Deleting refresh token for user.");
                await _refreshTokenRepository.DeleteAll(user.Id);

                _logger.LogInformation("Successfully logged out user everywhere.");
                return new NoContentResult();
            }
            catch (BearerSchemeNotProvidedException ex)
            {
                _logger.LogError(ex, "Bearer scheme not provided.");
                return new UnauthorizedResult();
            }
            catch (SecurityTokenDecryptionFailedException ex)
            {
                _logger.LogError(ex, "Failed to authenticate request.");
                return new UnauthorizedResult();
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Failed to authenticate request.");
                return new UnauthorizedResult();
            }
        }
    }
}
