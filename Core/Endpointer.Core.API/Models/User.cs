using System;

namespace Endpointer.Core.API.Models
{
    /// <summary>
    /// Model for an Endpointer user.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
