using Endpointer.Core.API.Models;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.UserRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get a user by email.
        /// </summary>
        /// <param name="email">The email of the user to find.</param>
        /// <returns>The user with the email. Null if not found.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<User> GetByEmail(string email);

        /// <summary>
        /// Get a user by username.
        /// </summary>
        /// <param name="username">The username of the user to find.</param>
        /// <returns>The user with the username. Null if not found.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<User> GetByUsername(string username);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user with a generated id.</returns>
        /// <exception cref="Exception">Thrown if create fails.</exception>
        Task Create(User user);

        /// <summary>
        /// Get a user by id.
        /// </summary>
        /// <param name="userId">The id of the user to find.</param>
        /// <returns>The user with the id. Null if not found.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<User> GetById(Guid userId);
    }
}
