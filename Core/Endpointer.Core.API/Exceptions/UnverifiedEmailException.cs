using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Core.API.Exceptions
{
    /// <summary>
    /// Exception for an unverified email on a user.
    /// </summary>
    public class UnverifiedEmailException : Exception
    {
        public string Email { get; set; }

        public UnverifiedEmailException(string email)
        {
            Email = email;
        }

        public UnverifiedEmailException(string message, string email) : base(message)
        {
            Email = email;
        }

        public UnverifiedEmailException(string message, Exception innerException, string email) : base(message, innerException)
        {
            Email = email;
        }
    }
}
