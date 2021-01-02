using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
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

        public DatabaseRefreshTokenRepository(TDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task Create(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteByToken(string refreshToken)
        {
            RefreshToken refreshTokenDTO = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (refreshTokenDTO != null)
            {
                _context.RefreshTokens.Remove(refreshTokenDTO);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task DeleteById(Guid id)
        {
            RefreshToken refreshToken = await _context.RefreshTokens.FindAsync(id);
            if(refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task DeleteAll(Guid userId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }
    }
}
