using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.RefreshTokenRepositories
{
    public class DatabaseRefreshTokenRepository<TDbContext> : IRefreshTokenRepository
        where TDbContext : DbContext, IAuthenticationDbContext<User>
    {
        private readonly TDbContext _context;
        private readonly ILogger<DatabaseRefreshTokenRepository<TDbContext>> _logger;

        public DatabaseRefreshTokenRepository(TDbContext context, ILogger<DatabaseRefreshTokenRepository<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task Create(RefreshToken refreshToken)
        {
            _logger.LogInformation("Saving refresh token.");
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully saved refresh token.");
        }

        /// <inheritdoc />
        public async Task DeleteByToken(string refreshToken)
        {
            _logger.LogInformation("Finding refresh token by token value to delete.");
            RefreshToken refreshTokenDTO = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            
            if (refreshTokenDTO != null)
            {
                _logger.LogInformation("Deleting refresh token.");
                _context.RefreshTokens.Remove(refreshTokenDTO);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted refresh token.");
            }
            else
            {
                _logger.LogWarning("Refresh token not found.");
            }
        }

        /// <inheritdoc />
        public async Task DeleteById(Guid id)
        {
            _logger.LogInformation("Finding refresh token by id to delete.");
            RefreshToken refreshToken = await _context.RefreshTokens.FindAsync(id);

            if(refreshToken != null)
            {
                _logger.LogInformation("Deleting refresh token.");
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted refresh token.");
            }
            else
            {
                _logger.LogWarning("Refresh token not found.");
            }
        }

        /// <inheritdoc />
        public async Task DeleteAll(Guid userId)
        {
            _logger.LogInformation("Finding all refresh tokens by user id to delete.");
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _logger.LogInformation("Deleting {RefreshTokenCount} user refresh tokens.", refreshTokens.Count());
            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted user refresh tokens.");
        }

        /// <inheritdoc />
        public async Task<RefreshToken> GetByToken(string token)
        {
            _logger.LogInformation("Finding refresh token by token value.");
            RefreshToken refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            if(refreshToken == null)
            {
                _logger.LogWarning("Refresh token not found.");
            }
            else
            {
                _logger.LogInformation("Successfully found refresh token by token value.");
            }

            return refreshToken;
        }
    }
}
