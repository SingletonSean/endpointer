using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Authentication.Client.Exceptions
{
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
