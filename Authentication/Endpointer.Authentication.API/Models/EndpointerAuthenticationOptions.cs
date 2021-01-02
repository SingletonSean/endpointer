using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    public class EndpointerAuthenticationOptions
    {
        public bool UseDatabase { get; set; }
        public Action<IServiceCollection> AddDbContext { get; set; }
        public Action<IServiceCollection> AddDbUserRepository { get; set; }
        public Action<IServiceCollection> AddDbRefreshTokenRepository { get; set; }
    }
}
