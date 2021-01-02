using Endpointer.Authentication.API.Models;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Get a stored refresh token by the refresh token value.
        /// </summary>
        /// <param name="token">The value of the refresh token.</param>
        /// <returns>The stored refresh token. Null if not found.</returns>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<RefreshToken> GetByToken(string token);

        /// <summary>
        /// Create a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to create.</param>
        /// <exception cref="Exception">Thrown if create fails.</exception>
        Task Create(RefreshToken refreshToken);

        /// <summary>
        /// Delete a refresh token by id.
        /// </summary>
        /// <param name="id">The id of the refresh token to delete.</param>
        /// <exception cref="Exception">Thrown if delete fails.</exception>
        Task DeleteById(Guid id);

        /// <summary>
        /// Delete a refresh token by the refresh token value.
        /// </summary>
        /// <param name="refreshToken">The value of the refresh token.</param>
        /// <exception cref="Exception">Thrown if delete fails.</exception>
        Task DeleteByToken(string refreshToken);

        /// <summary>
        /// Delete all of a user's refresh tokens.
        /// </summary>
        /// <param name="userId">The id of the target user.</param>
        /// <exception cref="Exception">Thrown if deletes fail.</exception>
        Task DeleteAll(Guid userId);
    }
}
