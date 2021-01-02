using Endpointer.Authentication.API.Models;
using System;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        /// <inheritdoc />
        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes));
        }
    }
}
