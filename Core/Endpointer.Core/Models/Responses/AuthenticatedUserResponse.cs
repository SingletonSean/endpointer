using System;

namespace Endpointer.Core.Models.Responses
{
    /// <summary>
    /// Model for an authenticated user's tokens.
    /// </summary>
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
    }
}
