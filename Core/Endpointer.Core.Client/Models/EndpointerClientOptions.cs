using Endpointer.Core.Client.Stores;
using Refit;
using System;

namespace Endpointer.Core.Client.Models
{
    /// <summary>
    /// Options for the Endpointer API client.
    /// </summary>
    public class EndpointerClientOptions
    {
        public RefitSettings RefitSettings { get; set; }

        public bool AutoTokenRefresh { get; set; }
        public Func<IServiceProvider, IAutoRefreshTokenStore> GetAutoRefreshTokenStore { get; set; }
    }
}
