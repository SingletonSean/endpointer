using System;

namespace Endpointer.Accounts.Client.Exceptions
{
    /// <summary>
    /// Exception for a failure to find an account.
    /// </summary>
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() { }

        public AccountNotFoundException(string message) : base(message) { }

        public AccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
