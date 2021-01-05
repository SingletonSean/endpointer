using System;

namespace Endpointer.Authentication.Client.Exceptions
{
    /// <summary>
    /// Exception for a non-matching confirm password.
    /// </summary>
    public class ConfirmPasswordException : Exception
    {
        public ConfirmPasswordException() {}

        public ConfirmPasswordException(string message) : base(message) { }

        public ConfirmPasswordException(string message, Exception innerException) : base(message, innerException) {}
    }
}
