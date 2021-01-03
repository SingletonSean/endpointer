using System;
using System.Collections.Generic;

namespace Endpointer.Core.Client.Exceptions
{
    public class ValidationFailedException : Exception
    {
        public IEnumerable<string> ValidationMessages { get; }

        public ValidationFailedException(string validationMessage) 
            : this(new string[] { validationMessage }) { }

        public ValidationFailedException(IEnumerable<string> validationMessages) 
            : this(validationMessages, null) { }

        public ValidationFailedException(string validationMessage, Exception innerException) 
            : this(new string[] { validationMessage }, innerException) { }

        public ValidationFailedException(IEnumerable<string> validationMessages, Exception innerException) 
            : base("Validation failed.", innerException)
        {
            ValidationMessages = validationMessages;
        }
    }
}
