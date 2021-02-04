using Endpointer.Authentication.API.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.RefreshTokenRepositories
{
    public class FirebaseRefreshTokenRepository : IRefreshTokenRepository
    {
        private const string DEFAULT_REFRESH_TOKEN_KEY = "refresh-tokens";

        private readonly string _refreshTokenKey;
        private readonly FirebaseClient _client;

        public FirebaseRefreshTokenRepository(FirebaseClient client)
        {
            _client = client;
            
            _refreshTokenKey = DEFAULT_REFRESH_TOKEN_KEY;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAll(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteByToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
