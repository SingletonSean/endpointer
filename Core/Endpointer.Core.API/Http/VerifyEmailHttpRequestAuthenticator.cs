using Endpointer.Core.API.Exceptions;
using Endpointer.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Endpointer.Core.API.Http
{
    public class VerifyEmailHttpRequestAuthenticator : IHttpRequestAuthenticator
    {
        private readonly IHttpRequestAuthenticator _baseAuthenticator;
        private readonly ILogger<VerifyEmailHttpRequestAuthenticator> _logger;

        public VerifyEmailHttpRequestAuthenticator(IHttpRequestAuthenticator baseAuthenticator, ILogger<VerifyEmailHttpRequestAuthenticator> logger)
        {
            _baseAuthenticator = baseAuthenticator;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate the request and check the user's email verification status.
        /// </summary>
        /// <inheritdoc />
        public async Task<User> Authenticate(HttpRequest request)
        {
            User user = await _baseAuthenticator.Authenticate(request);

            if(!user.IsEmailVerified)
            {
                _logger.LogError("User has unverified email.");
                throw new UnverifiedEmailException(user.Email);
            }

            return user;
        }
    }
}
