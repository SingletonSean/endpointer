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

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class RegisterEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterEndpointHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return new BadRequestObjectResult(new ErrorResponse(AuthenticationErrorCode.PASSWORDS_DO_NOT_MATCH, "Password does not match confirm password."));
            }

            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return new ConflictObjectResult(new ErrorResponse(AuthenticationErrorCode.EMAIL_ALREADY_EXISTS, "Email already exists."));
            }

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return new ConflictObjectResult(new ErrorResponse(AuthenticationErrorCode.USERNAME_ALREADY_EXISTS, "Username already exists."));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };

            await _userRepository.Create(registrationUser);

            return new OkResult();
        }
    }
}
