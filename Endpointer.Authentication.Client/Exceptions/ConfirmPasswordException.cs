using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Authentication.Client.Exceptions
{
    public class ConfirmPasswordException : Exception
    {
        public ConfirmPasswordException() {}

        public ConfirmPasswordException(string message) : base(message) { }

        public ConfirmPasswordException(string message, Exception innerException) : base(message, innerException) {}
    }
}
