using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for an authenticated Endpointer user.
    /// </summary>
    public class AuthenticatedUser
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
    }
}
