﻿using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Endpointer.Authentication.API.Services.TokenGenerators
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        /// <inheritdoc />
        public string GenerateToken(User user, DateTime expirationTime)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            };

            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims);
        }
    }
}