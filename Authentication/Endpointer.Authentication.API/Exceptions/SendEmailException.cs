using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Authentication.API.Exceptions
{
    /// <summary>
    /// Exception representing an email that failed to send.
    /// </summary>
    public class SendEmailException : Exception
    {
        public string To { get; }

        public SendEmailException(string to)
        {
            To = to;
        }

        public SendEmailException(string message, string to) : base(message)
        {
            To = to;
        }

        public SendEmailException(string message, Exception innerException, string to) : base(message, innerException)
        {
            To = to;
        }
    }
}
