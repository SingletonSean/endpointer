using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ILogger<TokenGenerator> _logger;

        public TokenGenerator(ILogger<TokenGenerator> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public string GenerateToken(string secretKey, string issuer, string audience, DateTime expires,
            IEnumerable<Claim> claims = null)
        {
            try
            {
                _logger.LogInformation("Creating token signing credentials.");
                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                _logger.LogInformation("Creating token.");
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    DateTime.UtcNow,
                    expires,
                    credentials);

                _logger.LogInformation("Writing token.");
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate token.");
                throw new Exception("Failed to generate token.", ex);
            }
        }
    }
}
