using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Authentication.Client.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }

        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
