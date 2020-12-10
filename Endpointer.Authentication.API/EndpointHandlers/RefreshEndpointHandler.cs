using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Models.Requests;
using Endpointer.Authentication.API.Models.Responses;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class RefreshEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;

        public RefreshEndpointHandler(IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository, 
            Authenticator authenticator, 
            RefreshTokenValidator refreshTokenValidator)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
        }

        public async Task<IActionResult> HandleRefresh(RefreshRequest refreshRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(new ErrorResponse(modelState));
            }

            return await HandleRefresh(refreshRequest);
        }

        public async Task<IActionResult> HandleRefresh(RefreshRequest refreshRequest)
        { 
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return new BadRequestObjectResult(new ErrorResponse("Invalid refresh token."));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return new NotFoundObjectResult(new ErrorResponse("Invalid refresh token."));
            }

            await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorResponse("User not found."));
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return new OkObjectResult(response);
        }
    }
}
