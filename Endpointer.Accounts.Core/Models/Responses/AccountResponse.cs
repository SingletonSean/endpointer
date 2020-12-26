using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Accounts.Core.Models.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
