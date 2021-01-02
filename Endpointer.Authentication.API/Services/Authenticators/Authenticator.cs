using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.TokenGenerators;
using Endpointer.Core.API.Models;
using Endpointer.Core.Models.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpointer.Authentication.API.Services.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthenticationConfiguration _configuration;
        private readonly ILogger<Authenticator> _log;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            AuthenticationConfiguration configuration,
            ILogger<Authenticator> log)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _log = log;
        }

        /// <inheritdoc />
        public async Task<AuthenticatedUser> Authenticate(User user)
        {
            _log.LogInformation("Authenticating {Username}.", user.Username);

            _log.LogInformation("Generating access token for {Username}.", user.Username);
            DateTime accessTokenExpirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
            string accessToken = _accessTokenGenerator.GenerateToken(user, accessTokenExpirationTime);

            _log.LogInformation("Generating refresh token for {Username}.", user.Username);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            _log.LogInformation("Saving refresh token for {Username}.", user.Username);
            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.Create(refreshTokenDTO);

            _log.LogInformation("Successfully authenticated {Username}.", user.Username);
            return new AuthenticatedUser()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpirationTime = accessTokenExpirationTime
            };
        }
    }
}
