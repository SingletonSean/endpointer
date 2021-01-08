namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for Endpointer authentication services.
    /// </summary>
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public double AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}
