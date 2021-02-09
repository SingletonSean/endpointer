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

        public EmailVerificationUserRegister(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            return user;
        }
    }
}
