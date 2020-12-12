using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Stores
{
    public class TokenStore
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
