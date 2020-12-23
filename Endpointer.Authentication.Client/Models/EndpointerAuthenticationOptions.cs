using Endpointer.Core.Client.Stores;
using Refit;
using System;

namespace Endpointer.Authentication.Client.Models
{
    public class EndpointerAuthenticationOptions
    {
        public RefitSettings RefitSettings { get; set; }

        public bool AutoTokenRefresh { get; set; }
        public Func<IServiceProvider, IAutoRefreshTokenStore> GetAutoRefreshTokenStore { get; set; }
    }
}
