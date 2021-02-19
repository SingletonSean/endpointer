using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for a decoded email verification token.
    /// </summary>
    public class EmailVerificationToken
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
