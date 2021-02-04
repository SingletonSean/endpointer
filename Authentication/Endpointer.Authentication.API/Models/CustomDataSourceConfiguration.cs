using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    /// <summary>
    /// Model for registering a custom data source.
    /// </summary>
    public class CustomDataSourceConfiguration
    {
        public Action<IServiceCollection> AddUserRepository { get; }
        public Action<IServiceCollection> AddRefreshTokenRepository { get; }

        public CustomDataSourceConfiguration(Action<IServiceCollection> addUserRepository, 
            Action<IServiceCollection> addRefreshTokenRepository)
        {
            AddUserRepository = addUserRepository;
            AddRefreshTokenRepository = addRefreshTokenRepository;
        }
    }
}
