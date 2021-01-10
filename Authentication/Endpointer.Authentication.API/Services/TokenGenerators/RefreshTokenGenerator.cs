using Endpointer.Authentication.API.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<RefreshTokenGenerator> _logger;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, 
            ITokenGenerator tokenGenerator,
            ILogger<RefreshTokenGenerator> logger)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        /// <inheritdoc />
        public string GenerateToken()
        {
            _logger.LogInformation("Generating refresh token.");
            return _tokenGenerator.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes));
        }
    }
}
