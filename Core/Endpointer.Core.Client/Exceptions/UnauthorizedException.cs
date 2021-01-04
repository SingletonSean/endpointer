using System;

namespace Endpointer.Core.Client.Exceptions
{
    /// <summary>
    /// Exception for an unauthorized API response.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }

        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
