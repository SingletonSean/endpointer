using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Models.Requests;
using Endpointer.Authentication.API.Models.Responses;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.PasswordHashers;
using Endpointer.Authentication.API.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LoginEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Authenticator _authenticator;

        public LoginEndpointHandler(IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            Authenticator authenticator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
        }

        public async Task<IActionResult> HandleLogin(LoginRequest loginRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(new ErrorResponse(modelState));
            }

            return await HandleLogin(loginRequest);
        }

        public async Task<IActionResult> HandleLogin(LoginRequest loginRequest)
        { 
            User user = await _userRepository.GetByUsername(loginRequest.Username);
            if (user == null)
            {
                return new UnauthorizedResult();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return new UnauthorizedResult();
            }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return new OkObjectResult(response);
        }
    }
}
