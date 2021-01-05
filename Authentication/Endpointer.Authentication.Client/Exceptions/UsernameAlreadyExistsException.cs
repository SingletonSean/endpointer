using System;

namespace Endpointer.Authentication.Client.Exceptions
{
    /// <summary>
    /// Exception for a username already existing.
    /// </summary>
    public class UsernameAlreadyExistsException : Exception
    {
        public string Username { get; }

        public UsernameAlreadyExistsException(string username)
        {
            Username = username;
        }

        public UsernameAlreadyExistsException(string username, Exception innerException) : base("Username already exists.", innerException)
        {
            Username = username;
        }

        public UsernameAlreadyExistsException(string username, string message, Exception innerException) : base(message, innerException)
        {
            Username = username;
        }
    }
}
