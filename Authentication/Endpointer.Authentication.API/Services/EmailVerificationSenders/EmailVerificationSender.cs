using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.EmailVerificationSenders
{
    public class EmailVerificationSender : IEmailVerificationSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailVerificationTokenGenerator _tokenGenerator;
        private readonly EmailVerificationConfiguration _configuration;
        private readonly ILogger<EmailVerificationSender> _logger;

        public EmailVerificationSender(IEmailSender emailSender, 
            IEmailVerificationTokenGenerator tokenGenerator, 
            EmailVerificationConfiguration configuration, 
            ILogger<EmailVerificationSender> logger)
        {
            _emailSender = emailSender;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _logger = logger;
        }

        ///<inheritdoc />
        public async Task SendEmailVerificationEmail(User user)
        {
            _logger.LogInformation("Creating email verification subject.");
            string subject = _configuration.CreateEmailSubject?.Invoke(user.Username);

            _logger.LogInformation("Generating email verification token.");
            string emailVerificationToken = _tokenGenerator.GenerateToken(user);
            string emailVerificationUrl = $"{_configuration.VerifyBaseUrl}?token={emailVerificationToken}";

            _logger.LogInformation("Sending email verification token to email for user {UserId}.", user.Id);
            await _emailSender.Send(
                _configuration.EmailFromAddress,
                _configuration.EmailFromName,
                user.Email,
                subject,
                $"Welcome {user.Username}! Please verify your email with the following link: {emailVerificationUrl}");

            _logger.LogInformation("Successfully sent email verification email.");
        }
    }
}
