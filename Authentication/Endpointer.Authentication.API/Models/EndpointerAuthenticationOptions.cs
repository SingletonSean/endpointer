using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for additional Endpointer Authentication options.
    /// </summary>
    public class EndpointerAuthenticationOptions
    {
        public bool RequireEmailVerification { get; set; }
        public EmailVerificationConfiguration EmailVerificationConfiguration { get; set; }

        public Action<IServiceCollection> AddDataSourceServices { get; set; }
    }
}
