using Endpointer.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Demos.WPF.Containers
{
    public static class AddViewsServiceCollectionExtensions
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services;
        }
    }
}
