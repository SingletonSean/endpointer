using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LogoutEndpointHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly HttpRequestAuthenticator _requestAuthenticator;

        public LogoutEndpointHandler(IRefreshTokenRepository refreshTokenRepository, HttpRequestAuthenticator requestAuthenticator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _requestAuthenticator = requestAuthenticator;
        }

        public async Task<IActionResult> HandleLogout(HttpRequest request)
        {
            try
            {
                User user = await _requestAuthenticator.Authenticate(request);

                await _refreshTokenRepository.DeleteAll(user.Id);

                return new NoContentResult();
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }
    }
}
