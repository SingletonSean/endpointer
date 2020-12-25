using Endpointer.Authentication.API.Models;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByToken(string token);

        Task Create(RefreshToken refreshToken);

        Task DeleteById(Guid id);
        
        Task DeleteByToken(string refreshToken);

        Task DeleteAll(Guid userId);
    }
}
