﻿using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
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

        public async Task<IActionResult> HandleLogout(string refreshToken)
        {
            await _refreshTokenRepository.DeleteByToken(refreshToken);

            return new NoContentResult();
        }
    }
}