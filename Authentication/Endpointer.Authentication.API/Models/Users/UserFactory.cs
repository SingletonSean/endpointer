using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Models.Users
{
    public class UserFactory : IUserFactory
    {
        /// <inheritdoc />
        public User CreateUser(string email, string username, string passwordHash)
        {
            return new User()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                IsEmailVerified = true
            };
        }
    }
}
