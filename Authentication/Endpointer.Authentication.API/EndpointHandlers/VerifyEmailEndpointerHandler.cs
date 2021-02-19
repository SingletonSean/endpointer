using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class VerifyEmailEndpointerHandler
    {
        private readonly IEmailVerificationTokenValidator _tokenValidator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<VerifyEmailEndpointerHandler> _logger;

        public VerifyEmailEndpointerHandler(IEmailVerificationTokenValidator tokenValidator, 
            IUserRepository userRepository, 
            ILogger<VerifyEmailEndpointerHandler> logger)
        {
            _tokenValidator = tokenValidator;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handle a verify email request with model state validation.
        /// </summary>
        /// <param name="verifyRequest">The request to verify an email.</param>
        /// <param name="modelState">The request model state to validate.</param>
        /// <returns>The result of verifying the email.</returns>
        /// <exception cref="Exception">Thrown if verification fails.</exception>
        public async Task<IActionResult> HandleVerifyEmail(VerifyEmailRequest verifyRequest, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                _logger.LogError("Request has invalid model state.");
                return new BadRequestObjectResult(modelState.CreateErrorResponse());
            }

            return await HandleVerifyEmail(verifyRequest);
        }

        /// <summary>
        /// Handle a verify email request.
        /// </summary>
        /// <param name="verifyRequest">The request to verify an email.</param>
        /// <returns>The result of verifying the email.</returns>
        /// <exception cref="Exception">Thrown if verification fails.</exception>
        public async Task<IActionResult> HandleVerifyEmail(VerifyEmailRequest verifyRequest)
        {
            try
            {
                _logger.LogInformation("Validating email verification token.");
                EmailVerificationToken token = _tokenValidator.Validate(verifyRequest.VerifyToken);

                _logger.LogInformation("Updating user's email verification status.");
                await _userRepository.Update(token.UserId, (u) => u.IsEmailVerified = true);

                _logger.LogInformation("Successfully verified user email.");

                return new OkResult();
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError("Failed to validate email verification token.", ex);

                return new UnauthorizedResult();
            }
        }
    }
}
