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
        private readonly HttpRequestAuthenticator _requestAuthenticator;

        public LogoutEverywhereEndpointHandler(IRefreshTokenRepository refreshTokenRepository, HttpRequestAuthenticator requestAuthenticator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _requestAuthenticator = requestAuthenticator;
        }

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
