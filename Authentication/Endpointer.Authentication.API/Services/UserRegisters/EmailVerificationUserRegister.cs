using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.EmailSenders;
using Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
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

        public EmailVerificationUserRegister(IUserRepository userRepository,
            IEmailSender emailSender,
            IEmailVerificationTokenGenerator tokenGenerator,
            EmailVerificationConfiguration emailVerificationConfiguration) 
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _tokenGenerator = tokenGenerator;
            _emailVerificationConfiguration = emailVerificationConfiguration;
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

            await _userRepository.Create(user);

            string subject = _emailVerificationConfiguration.CreateEmailSubject?.Invoke(username);

            string emailVerificationToken = _tokenGenerator.GenerateToken(email);
            string emailVerificationUrl = $"{_emailVerificationConfiguration.VerifyBaseUrl}?token={emailVerificationToken}";

            await _emailSender.Send(
                _emailVerificationConfiguration.EmailFromAddress, 
                email, 
                subject, 
                $"Welcome {username}! Please verify your email with the following link: {emailVerificationUrl}");

            return user;
        }
    }
}
