using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Models.Users
{
    public interface IUserFactory
    {
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="passwordHash">The user's hashed password.</param>
        /// <returns>The created user.</returns>
        User CreateUser(string email, string username, string passwordHash);
    }
}
