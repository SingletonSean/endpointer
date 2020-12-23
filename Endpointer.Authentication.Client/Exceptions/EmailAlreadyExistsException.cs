using System;

namespace Endpointer.Authentication.Client.Exceptions
{
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
