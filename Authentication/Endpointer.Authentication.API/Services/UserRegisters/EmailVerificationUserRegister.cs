using Endpointer.Authentication.API.Exceptions;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Services.EmailVerificationSenders;
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
        private readonly IEmailVerificationSender _emailSender;
        private readonly ILogger<EmailVerificationUserRegister> _logger;

        public EmailVerificationUserRegister(IUserRepository userRepository,
            IEmailVerificationSender emailSender,
            ILogger<EmailVerificationUserRegister> logger)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
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
                _logger.LogInformation("Sending email verification email.");
                await _emailSender.SendEmailVerificationEmail(user);

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
