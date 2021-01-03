using System;

namespace Endpointer.Core.Client.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() { }

        public InvalidRefreshTokenException(string message) : base(message) { }

        public InvalidRefreshTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
