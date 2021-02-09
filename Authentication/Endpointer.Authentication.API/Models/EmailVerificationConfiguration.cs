using System;
using System.Collections.Generic;
using System.Text;

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

        public string EmailVerificationTokenSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime EmailVerificationTokenExpirationTime { get; set; }

        public EmailVerificationConfiguration()
        {
            CreateEmailSubject = (username) => "Verify Email";
        }
    }
}
