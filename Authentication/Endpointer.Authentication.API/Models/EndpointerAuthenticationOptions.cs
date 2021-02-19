using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for additional Endpointer Authentication options.
    /// </summary>
    public class EndpointerAuthenticationOptions
    {
        public bool EnableEmailVerification { get; set; }
        public bool RequireVerifiedEmail { get; set; }
        public EmailVerificationConfiguration EmailVerificationConfiguration { get; set; }

        public Action<IServiceCollection> AddDataSourceServices { get; set; }
    }
}
