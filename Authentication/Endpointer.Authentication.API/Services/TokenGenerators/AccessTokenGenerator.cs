using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<AccessTokenGenerator> _logger;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, 
            ITokenGenerator tokenGenerator,
            ILogger<AccessTokenGenerator> logger)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        /// <inheritdoc />
        public string GenerateToken(User user, DateTime expirationTime)
        {
            _logger.LogInformation("Creating claims for user.");
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            };

            _logger.LogInformation("Generating access token for user claims.");
            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims);
        }
    }
}
