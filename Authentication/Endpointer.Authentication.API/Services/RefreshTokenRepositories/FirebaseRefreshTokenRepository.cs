using Endpointer.Authentication.API.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
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

        ///<inheritdoc />
        public async Task Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();

            await _client
                .Child(_refreshTokenKey)
                .Child(refreshToken.Id.ToString())
                .PutAsync(refreshToken);
        }

        ///<inheritdoc />
        public async Task DeleteAll(Guid userId)
        {
            IReadOnlyCollection<FirebaseObject<RefreshToken>> userRefreshTokens = await _client
                .Child(_refreshTokenKey)
                .OrderBy(nameof(RefreshToken.UserId))
                .EqualTo(userId.ToString())
                .OnceAsync<RefreshToken>();

            foreach (FirebaseObject<RefreshToken> firebaseRefreshToken in userRefreshTokens)
            {
                RefreshToken refreshToken = firebaseRefreshToken.Object;
                if(refreshToken != null)
                {
                    await DeleteById(refreshToken.Id);
                }
            }
        }

        ///<inheritdoc />
        public async Task DeleteById(Guid id)
        {
            await _client
                .Child(_refreshTokenKey)
                .Child(id.ToString())
                .DeleteAsync();
        }

        ///<inheritdoc />
        public async Task DeleteByToken(string refreshToken)
        {
            RefreshToken token = await GetByToken(refreshToken);

            await DeleteById(token.Id);
        }

        ///<inheritdoc />
        public async Task<RefreshToken> GetByToken(string token)
        {
            IReadOnlyCollection<FirebaseObject<RefreshToken>> users = await _client
                .Child(_refreshTokenKey)
                .OrderBy(nameof(RefreshToken.Token))
                .EqualTo(token)
                .OnceAsync<RefreshToken>();

            RefreshToken user = users.FirstOrDefault()?.Object;

            if (user == null || user.Token != token)
            {
                return null;
            }

            return user;
        }
    }
}
