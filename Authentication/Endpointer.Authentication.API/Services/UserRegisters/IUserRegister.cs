using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRegisters
{
    public interface IUserRegister
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="passwordHash">The user's hashed password.</param>
        /// <returns>The created user.</returns>
        /// <exception cref="Exception">Thrown if registration fails.</exception>
        Task<User> RegisterUser(string email, string username, string passwordHash);
    }
}
