using Endpointer.Authentication.API.Exceptions;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRegisters
{
    public class EmailVerificationUserRegister : IUserRegister
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailVerificationTokenGenerator _tokenGenerator;
        private readonly EmailVerificationConfiguration _emailVerificationConfiguration;
        private readonly ILogger<EmailVerificationUserRegister> _logger;

        public EmailVerificationUserRegister(IUserRepository userRepository,
            IEmailSender emailSender,
            IEmailVerificationTokenGenerator tokenGenerator,
            EmailVerificationConfiguration emailVerificationConfiguration,
            ILogger<EmailVerificationUserRegister> logger)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _tokenGenerator = tokenGenerator;
            _emailVerificationConfiguration = emailVerificationConfiguration;
            _logger = logger;
        }

        ///<inheritdoc />
        public async Task<User> RegisterUser(string email, string username, string passwordHash)
        {
            User user = new User()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                IsEmailVerified = false
            };

            _logger.LogInformation("Creating new user with unverified email.");
            await _userRepository.Create(user);

            try
            {
                _logger.LogInformation("Creating email verification subject.");
                string subject = _emailVerificationConfiguration.CreateEmailSubject?.Invoke(username);

                _logger.LogInformation("Generating email verification token.");
                string emailVerificationToken = _tokenGenerator.GenerateToken(user);
                string emailVerificationUrl = $"{_emailVerificationConfiguration.VerifyBaseUrl}?token={emailVerificationToken}";

                _logger.LogInformation("Sending email verification token to new user's email.");
                await _emailSender.Send(
                    _emailVerificationConfiguration.EmailFromAddress,
                    email,
                    subject,
                    $"Welcome {username}! Please verify your email with the following link: {emailVerificationUrl}");

                _logger.LogInformation("Successfully registered user.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Successfully registered user, but failed to send email verification email.", ex);
                throw new SendEmailException("Failed to send email verification email.", ex, email);
            }
        }
    }
}
