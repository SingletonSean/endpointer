using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Services.TokenGenerators.EmailVerifications
{
    public interface IEmailVerificationTokenGenerator
    {
        /// <summary>
        /// Generate an email verification token for an email.
        /// </summary>
        /// <param name="email">The email to generate the token for.</param>
        /// <returns>The generated token.</returns>
        /// <exception cref="Exception">Thrown if token generation fails.</exception>
        string GenerateToken(string email);
    }
}
