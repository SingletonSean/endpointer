using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthenticationConfiguration _configuration;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator, 
            IRefreshTokenRepository refreshTokenRepository,
            AuthenticationConfiguration configuration)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            DateTime accessTokenExpirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
            string accessToken = _accessTokenGenerator.GenerateToken(user, accessTokenExpirationTime);
            
            string refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.Create(refreshTokenDTO);

            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpirationTime = accessTokenExpirationTime
            };
        }
    }
}
