using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Core.API.Exceptions
{
    /// <summary>
    /// Exception for missing authorization header bearer scheme.
    /// </summary>
    public class BearerSchemeNotProvidedException : Exception
    {
        public string AuthorizationHeader { get; }

        public BearerSchemeNotProvidedException() : base() { }

        public BearerSchemeNotProvidedException(string authorizationHeader) : base()
        {
            AuthorizationHeader = authorizationHeader;
        }

        public BearerSchemeNotProvidedException(string message, string authorizationHeader) : base(message) 
        {
            AuthorizationHeader = authorizationHeader;
        }
    }
}
