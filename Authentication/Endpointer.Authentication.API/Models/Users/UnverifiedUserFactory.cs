using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Models.Users
{
    public class UnverifiedUserFactory : IUserFactory
    {
        ///<summary>
        /// Create an unverified user.
        /// </summary>
        /// <inheritdoc />
        public User CreateUser(string email, string username, string passwordHash)
        {
            return new User()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                IsEmailVerified = false
            };
        }
    }
}
