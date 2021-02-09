using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRegisters
{
    public class UserRegister : IUserRegister
    {
        private readonly IUserRepository _userRepository;

        public UserRegister(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            user = await _userRepository.Create(user);

            return user;
        }
    }
}
