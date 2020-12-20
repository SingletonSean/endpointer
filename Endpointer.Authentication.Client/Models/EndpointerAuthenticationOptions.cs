using Endpointer.Authentication.Client.Services;
using Endpointer.Core.Client.Services;
using Endpointer.Core.Client.Stores;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Client.Models
{
    public class EndpointerAuthenticationOptions
    {
        public RefitSettings RefitSettings { get; set; }

        public bool AutoTokenRefresh { get; set; }
        public Func<IServiceProvider, IAutoRefreshTokenStore> GetAutoRefreshTokenStore { get; set; }
    }
}
