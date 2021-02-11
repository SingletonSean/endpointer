using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRegisters
{
    public class UserRegister : IUserRegister
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserRegister> _logger;

        public UserRegister(IUserRepository userRepository, ILogger<UserRegister> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> RegisterUser(string email, string username, string passwordHash)
        {
            User user = new User()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                IsEmailVerified = true
            };

            _logger.LogInformation("Creating new user with verified email.");
            await _userRepository.Create(user);

            _logger.LogInformation("Successfully registered user.");
            return user;
        }
    }
}
