﻿using AutoMapper;
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

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class LoginEndpointHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticator _authenticator;
        private readonly IMapper _mapper;

        public LoginEndpointHandler(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAuthenticator authenticator, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _mapper = mapper;
        }

        public async Task<IActionResult> HandleLogin(LoginRequest loginRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
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

            AuthenticatedUser authenticatedUser = await _authenticator.Authenticate(user);
            AuthenticatedUserResponse response = _mapper.Map<AuthenticatedUserResponse>(authenticatedUser);

            return new OkObjectResult(new SuccessResponse<AuthenticatedUserResponse>(response));
        }
    }
}