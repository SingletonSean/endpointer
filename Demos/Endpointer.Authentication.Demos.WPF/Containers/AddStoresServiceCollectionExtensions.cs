using Endpointer.Authentication.Demos.WPF.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Authentication.Demos.WPF.Containers
{
    public static class AddStoresServiceCollectionExtensions
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<TokenStore>();

            return services;
        }
    }
}
