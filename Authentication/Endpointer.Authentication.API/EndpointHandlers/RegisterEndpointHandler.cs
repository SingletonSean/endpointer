using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Core.API.Extensions;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.Services.UserRegisters;
using Endpointer.Authentication.API.Exceptions;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class RegisterEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRegister _userRegister;
        private readonly ILogger<RegisterEndpointHandler> _logger;

        public RegisterEndpointHandler(IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IUserRegister userRegister,
            ILogger<RegisterEndpointHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userRegister = userRegister;
            _logger = logger;
        }

        /// <summary>
        /// Handle a register with model state validation.
        /// </summary>
        /// <param name="registerRequest">The request to register.</param>
        /// <param name="modelState">The request model state to validate.</param>
        /// <returns>The result of registering.</returns>
        /// <exception cref="Exception">Thrown if register fails.</exception>
        public async Task<IActionResult> HandleRegister(RegisterRequest registerRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                _logger.LogError("Request has invalid model state.");
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
            }

            return await HandleRegister(registerRequest);
        }

        /// <summary>
        /// Handle a register.
        /// </summary>
        /// <param name="registerRequest">The request to register.</param>
        /// <returns>The result of registering.</returns>
        /// <exception cref="Exception">Thrown if register fails.</exception>
        public async Task<IActionResult> HandleRegister(RegisterRequest registerRequest)
        {
            _logger.LogInformation("Comparing password and confirm password.");
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return new BadRequestObjectResult(new ErrorResponse(AuthenticationErrorCode.PASSWORDS_DO_NOT_MATCH, "Password does not match confirm password."));
            }

            _logger.LogInformation("Ensuring email {Email} is available.", registerRequest.Email);
            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);

            if (existingUserByEmail != null)
            {
                _logger.LogError("Email {Email} already exists.", registerRequest.Email);
                return new ConflictObjectResult(new ErrorResponse(AuthenticationErrorCode.EMAIL_ALREADY_EXISTS, "Email already exists."));
            }

            _logger.LogInformation("Ensuring username {Username} is available.", registerRequest.Username);
            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);

            if (existingUserByUsername != null)
            {
                _logger.LogError("Username {Username} already exists.", registerRequest.Username);
                return new ConflictObjectResult(new ErrorResponse(AuthenticationErrorCode.USERNAME_ALREADY_EXISTS, "Username already exists."));
            }

            _logger.LogInformation("Hashing user password.");
            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

            try
            {
                _logger.LogInformation("Registering new user.");
                await _userRegister.RegisterUser(registerRequest.Email, registerRequest.Username, passwordHash);

                _logger.LogInformation("Successfully registered user.");
                return new OkResult();
            }
            catch (SendEmailException)
            {
                _logger.LogError("Failed to send email verification.");
                return new OkResult();
            }
        }
    }
}
