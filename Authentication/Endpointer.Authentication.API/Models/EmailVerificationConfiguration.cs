using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model to configure email verification.
    /// </summary>
    public class EmailVerificationConfiguration
    {
        public string EmailFromAddress { get; set; }

        /// <summary>
        /// Callback to create an email subject using the new user's username.
        /// </summary>
        public Func<string, string> CreateEmailSubject { get; set; }

        public string VerifyBaseUrl { get; set; }
        public string TokenSecret { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public double TokenExpirationMinutes { get; set; }
        public Action<FluentEmailServicesBuilder> ConfigureFluentEmailServices { get; set; }

        public EmailVerificationConfiguration()
        {
            CreateEmailSubject = (username) => "Verify Email";
        }
    }
}
