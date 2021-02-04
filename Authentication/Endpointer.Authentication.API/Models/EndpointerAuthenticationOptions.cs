using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for additional Endpointer Authentication options.
    /// </summary>
    public class EndpointerAuthenticationOptions
    {
        public Action<IServiceCollection> AddDataSourceServices { get; set; }
    }
}
