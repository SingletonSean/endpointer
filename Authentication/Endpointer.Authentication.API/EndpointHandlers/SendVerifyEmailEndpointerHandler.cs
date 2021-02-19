using Endpointer.Authentication.API.Services.EmailVerificationSenders;
using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Http;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.EndpointHandlers
{
    public class SendVerifyEmailEndpointerHandler
    {
        private readonly IHttpRequestAuthenticator _authenticator;
        private readonly IEmailVerificationSender _emailSender;
        private readonly ILogger<SendVerifyEmailEndpointerHandler> _logger;

        public SendVerifyEmailEndpointerHandler(IHttpRequestAuthenticator authenticator,
            IEmailVerificationSender emailSender,
            ILogger<SendVerifyEmailEndpointerHandler> logger)
        {
            _authenticator = authenticator;
            _emailSender = emailSender;
            _logger = logger;
        }

        /// <summary>
        /// Send an email verification email to a user.
        /// </summary>
        /// <param name="request">The request with the user requesting the email.</param>
        /// <returns>The result of the email request.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        public async Task<IActionResult> HandleSendVerifyEmail(HttpRequest request)
        {
            try
            {
                _logger.LogInformation("Authenticating user.");
                User user = await _authenticator.Authenticate(request);

                _logger.LogInformation("Sending email verification email.");
                await _emailSender.SendEmailVerificationEmail(user);

                return new OkResult();
            }
            catch (BearerSchemeNotProvidedException ex)
            {
                _logger.LogError("Bearer scheme not provided.", ex);
                return new UnauthorizedResult();
            }
            catch(SecurityTokenException ex)
            {
                _logger.LogError("Invalid access token.", ex);
                return new UnauthorizedResult();
            }
        }
    }
}
