using AutoMapper;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenValidators;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Extensions;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class RefreshEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthenticator _authenticator;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<RefreshEndpointHandler> _logger;

        public RefreshEndpointHandler(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthenticator authenticator,
            IRefreshTokenValidator refreshTokenValidator,
            IMapper mapper, 
            ILogger<RefreshEndpointHandler> logger)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handle a refresh with model state validation.
        /// </summary>
        /// <param name="refreshRequest">The request to refresh.</param>
        /// <param name="modelState">The request model state to validate.</param>
        /// <returns>The result of refreshing.</returns>
        /// <exception cref="Exception">Thrown if refresh fails.</exception>
        public async Task<IActionResult> HandleRefresh(RefreshRequest refreshRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                _logger.LogError("Request has invalid model state.");
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
            }

            return await HandleRefresh(refreshRequest);
        }

        /// <summary>
        /// Handle a refresh.
        /// </summary>
        /// <param name="refreshRequest">The request to refresh.</param>
        /// <returns>The result of refreshing.</returns>
        /// <exception cref="Exception">Thrown if refresh fails.</exception>
        public async Task<IActionResult> HandleRefresh(RefreshRequest refreshRequest)
        { 
            _logger.LogInformation("Validating refresh token.");
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if (!isValidRefreshToken)
            {
                _logger.LogError("Invalid refresh token.");
                return new BadRequestObjectResult(new ErrorResponse(ErrorCode.INVALID_REFRESH_TOKEN, "Invalid refresh token."));
            }

            _logger.LogInformation("Finding stored refresh token.");
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);

            if (refreshTokenDTO == null)
            {
                _logger.LogError("Refresh token not found. Invalid refresh token.");
                return new NotFoundObjectResult(new ErrorResponse(ErrorCode.INVALID_REFRESH_TOKEN, "Invalid refresh token."));
            }

            _logger.LogInformation("Deleting refresh token.");
            await _refreshTokenRepository.DeleteById(refreshTokenDTO.Id);

            _logger.LogInformation("Finding refresh token user.");
            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                _logger.LogError("Refresh token user not found.");
                return new NotFoundResult();
            }

            _logger.LogInformation("Authenticating user.");
            AuthenticatedUser authenticatedUser = await _authenticator.Authenticate(user);
            AuthenticatedUserResponse response = _mapper.Map<AuthenticatedUserResponse>(authenticatedUser);

            _logger.LogInformation("Successfully refreshed tokens.");
            return new OkObjectResult(new SuccessResponse<AuthenticatedUserResponse>(response));
        }
    }
}
