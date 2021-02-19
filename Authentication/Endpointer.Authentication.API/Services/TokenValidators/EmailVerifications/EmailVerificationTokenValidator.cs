using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Services.TokenClaimsDecoders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications
{
    public class EmailVerificationTokenValidator : IEmailVerificationTokenValidator
    {
        private readonly ITokenClaimsDecoder _tokenDecoder;
        private readonly EmailVerificationConfiguration _configuration;
        private readonly ILogger<EmailVerificationTokenValidator> _logger;

        public EmailVerificationTokenValidator(ITokenClaimsDecoder tokenDecoder, 
            EmailVerificationConfiguration configuration,
            ILogger<EmailVerificationTokenValidator> logger)
        {
            _tokenDecoder = tokenDecoder;
            _configuration = configuration;
            _logger = logger;
        }

        /// <inheritdoc />
        public EmailVerificationToken Validate(string emailVerificationToken)
        {
            _logger.LogInformation("Setting up email verification token validation parameters.");
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.TokenSecret)),
                ValidIssuer = _configuration.TokenIssuer,
                ValidAudience = _configuration.TokenAudience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            _logger.LogInformation("Decoding token claims.");
            ClaimsPrincipal claims = _tokenDecoder.GetClaims(emailVerificationToken, validationParameters);

            _logger.LogInformation("Getting user id from email verification token.");
            Guid userId = GetId(claims);

            _logger.LogInformation("Getting email from email verification token.");
            string email = GetEmail(claims);

            _logger.LogInformation("Successfully decoded email verification token.");

            return new EmailVerificationToken()
            {
                UserId = userId,
                Email = email
            };
        }

        private string GetEmail(ClaimsPrincipal claims)
        {
            Claim emailClaim = claims.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                _logger.LogError("Failed to parse user email from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse email from JWT.");
            }

            return emailClaim.Value;
        }

        private Guid GetId(ClaimsPrincipal claims)
        {
            string rawId = claims.FindFirst("id")?.Value;
            if (!Guid.TryParse(rawId, out Guid id))
            {
                _logger.LogError("Failed to parse user id from token claims.");
                throw new SecurityTokenDecryptionFailedException("Unable to parse ID from JWT.");
            }

            return id;
        }
    }
}
