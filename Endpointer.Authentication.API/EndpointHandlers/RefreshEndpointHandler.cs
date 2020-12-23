using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
            }

            return await HandleRefresh(refreshRequest);
        }

        public async Task<IActionResult> HandleRefresh(RefreshRequest refreshRequest)
        { 
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return new BadRequestObjectResult(new ErrorResponse(ErrorCode.INVALID_REFRESH_TOKEN, "Invalid refresh token."));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(ErrorCode.INVALID_REFRESH_TOKEN, "Invalid refresh token."));
            }

            await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return new NotFoundResult();
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return new OkObjectResult(new SuccessResponse<AuthenticatedUserResponse>(response));
        }
    }
}
