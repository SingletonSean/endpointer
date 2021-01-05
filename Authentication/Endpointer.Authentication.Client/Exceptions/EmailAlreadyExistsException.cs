using System;

namespace Endpointer.Authentication.Client.Exceptions
{
    /// <summary>
    /// Exception for an email already existing.
    /// </summary>
    public class EmailAlreadyExistsException : Exception
    {
        public string Email { get; }

        public EmailAlreadyExistsException(string email)
        {
            Email = email;
        }

        public EmailAlreadyExistsException(string email, Exception innerException) : base("Email already exists.", innerException)
        {
            Email = email;
        }

        public EmailAlreadyExistsException(string email, string message, Exception innerException) : base(message, innerException)
        {
            Email = email;
        }
    }
}
