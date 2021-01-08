using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LogoutEverywhereEndpointHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpRequestAuthenticator _requestAuthenticator;

        public LogoutEverywhereEndpointHandler(IRefreshTokenRepository refreshTokenRepository, IHttpRequestAuthenticator requestAuthenticator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _requestAuthenticator = requestAuthenticator;
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
                User user = await _requestAuthenticator.Authenticate(request);

                await _refreshTokenRepository.DeleteAll(user.Id);

                return new NoContentResult();
            }
            catch (BearerSchemeNotProvidedException)
            {
                return new UnauthorizedResult();
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return new UnauthorizedResult();
            }
            catch (SecurityTokenException)
            {
                return new UnauthorizedResult();
            }
        }
    }
}
