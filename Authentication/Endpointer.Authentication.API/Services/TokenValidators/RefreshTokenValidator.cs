using Endpointer.Authentication.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Endpointer.Authentication.API.Services.TokenValidators
{
    public class RefreshTokenValidator : IRefreshTokenValidator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ILogger<RefreshTokenValidator> _logger;

        public RefreshTokenValidator(AuthenticationConfiguration configuration, ILogger<RefreshTokenValidator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <inheritdoc />
        public bool Validate(string refreshToken)
        {
            _logger.LogInformation("Creating token validation parameters.");
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret)),
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _logger.LogInformation("Validating token.");
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

                _logger.LogInformation("Successfully validated token.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                return false;
            }
        }
    }
}
