using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Endpointer.Authentication.Client.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> ValidationMessages { get; }

        public ValidationException(string validationMessage) 
            : this(new string[] { validationMessage }) { }

        public ValidationException(IEnumerable<string> validationMessages) 
            : this(validationMessages, null) { }

        public ValidationException(string validationMessage, Exception innerException) 
            : this(new string[] { validationMessage }, innerException) { }

        public ValidationException(IEnumerable<string> validationMessages, Exception innerException) 
            : base("Validation failed.", innerException)
        {
            ValidationMessages = validationMessages;
        }
    }
}
