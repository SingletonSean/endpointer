using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for a refresh token.
    /// </summary>
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
