namespace Endpointer.Authentication.Client.Models
{
    /// <summary>
    /// Model for API endpoint URIs.
    /// </summary>
    public class AuthenticationEndpointsConfiguration
    {
        public string RegisterEndpoint { get; set; }
        public string LoginEndpoint { get; set; }
        public string RefreshEndpoint { get; set; }
        public string LogoutEndpoint { get; set; }
    }
}
