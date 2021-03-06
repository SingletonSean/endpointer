﻿using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications
{
    public class EmailVerificationTokenGenerator : IEmailVerificationTokenGenerator
    {
        private readonly EmailVerificationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<EmailVerificationTokenGenerator> _logger;

        public EmailVerificationTokenGenerator(EmailVerificationConfiguration configuration, 
            ITokenGenerator tokenGenerator,
            ILogger<EmailVerificationTokenGenerator> logger)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        /// <inheritdoc />
        public string GenerateToken(User user)
        {
            _logger.LogInformation("Creating email verification claims.");
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            _logger.LogInformation("Generating email verification token for email claim.");
            return _tokenGenerator.GenerateToken(
                _configuration.TokenSecret,
                _configuration.TokenIssuer,
                _configuration.TokenAudience,
                DateTime.UtcNow.AddMinutes(_configuration.TokenExpirationMinutes),
                claims);
        }
    }
}
