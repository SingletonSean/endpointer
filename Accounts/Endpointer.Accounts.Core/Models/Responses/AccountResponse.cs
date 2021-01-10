using System;

namespace Endpointer.Accounts.Core.Models.Responses
{
    /// <summary>
    /// Model for an account response from the API.
    /// </summary>
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
