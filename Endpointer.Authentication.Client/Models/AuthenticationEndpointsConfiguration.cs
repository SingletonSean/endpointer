using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Client.Models
{
    public class AuthenticationEndpointsConfiguration
    {
        public string RegisterEndpoint { get; set; }
        public string LoginEndpoint { get; set; }
        public string RefreshEndpoint { get; set; }
        public string LogoutEndpoint { get; set; }
    }
}
