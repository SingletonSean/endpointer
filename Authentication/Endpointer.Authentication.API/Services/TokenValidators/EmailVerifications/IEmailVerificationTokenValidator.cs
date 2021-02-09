using System;
using System.Collections.Generic;
using System.Text;
using Endpointer.Authentication.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace Endpointer.Authentication.API.Services.TokenValidators.EmailVerifications
{
    public interface IEmailVerificationTokenValidator
    {
        /// <summary>
        /// Validate a JWT email verification token.
        /// </summary>
        /// <param name="emailVerificationToken">The token to validate.</param>
        /// <returns>The decoded email token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if token validation fails.</exception>
        EmailVerificationToken Validate(string emailVerificationToken);
    }
}
