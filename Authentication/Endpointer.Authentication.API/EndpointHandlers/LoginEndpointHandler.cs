using AutoMapper;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.API.Extensions;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LoginEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticator _authenticator;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginEndpointHandler> _logger;

        public LoginEndpointHandler(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAuthenticator authenticator,
            IMapper mapper, 
            ILogger<LoginEndpointHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handle a login with model state validation.
        /// </summary>
        /// <param name="loginRequest">The request to login.</param>
        /// <param name="modelState">The request model state to validate.</param>
        /// <returns>The result of logging in.</returns>
        /// <exception cref="Exception">Thrown if login fails.</exception>
        public async Task<IActionResult> HandleLogin(LoginRequest loginRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                _logger.LogError("Request has invalid model state.");
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
            }

            return await HandleLogin(loginRequest);
        }

        /// <summary>
        /// Handle a login.
        /// </summary>
        /// <param name="loginRequest">The request to login.</param>
        /// <returns>The result of logging in.</returns>
        /// <exception cref="Exception">Thrown if login fails.</exception>
        public async Task<IActionResult> HandleLogin(LoginRequest loginRequest)
        {
            _logger.LogInformation("Finding login user.");
            User user = await _userRepository.GetByUsername(loginRequest.Username);
            if (user == null)
            {
                _logger.LogError("Failed to find user with username {Username}.", loginRequest.Username);
                return new UnauthorizedResult();
            }

            _logger.LogInformation("Verifying login user password.");
            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                _logger.LogError("User provided incorrect password.");
                return new UnauthorizedResult();
            }

            _logger.LogInformation("Authenticating user.");
            AuthenticatedUser authenticatedUser = await _authenticator.Authenticate(user);
            AuthenticatedUserResponse response = _mapper.Map<AuthenticatedUserResponse>(authenticatedUser);

            _logger.LogInformation("Successfully logged in.");
            return new OkObjectResult(new SuccessResponse<AuthenticatedUserResponse>(response));
        }
    }
}
