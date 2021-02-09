using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Endpointer.Authentication.API.Services.TokenValidators
{
    public class RefreshTokenValidator : IRefreshTokenValidator
    {
        private readonly ITokenClaimsDecoder _tokenDecoder;
        private readonly AuthenticationConfiguration _configuration;
        private readonly ILogger<RefreshTokenValidator> _logger;

        public RefreshTokenValidator(ITokenClaimsDecoder tokenDecoder, AuthenticationConfiguration configuration, ILogger<RefreshTokenValidator> logger)
        {
            _tokenDecoder = tokenDecoder;
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

            try
            {
                _logger.LogInformation("Validating token.");
                _tokenDecoder.GetClaims(refreshToken, validationParameters);

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
