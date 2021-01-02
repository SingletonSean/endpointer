using System;

namespace Endpointer.Authentication.API.Models
{
    public class AuthenticatedUser
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
    }
}
