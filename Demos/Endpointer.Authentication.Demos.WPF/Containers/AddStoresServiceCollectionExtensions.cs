using Endpointer.Demos.WPF.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Demos.WPF.Containers
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
