using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for additional Endpointer Authentication options.
    /// </summary>
    public class EndpointerAuthenticationOptions
    {
        public bool UseDatabase { get; set; }
        public Action<IServiceCollection> AddDbContext { get; set; }
        public Action<IServiceCollection> AddDbUserRepository { get; set; }
        public Action<IServiceCollection> AddDbRefreshTokenRepository { get; set; }
    }
}
