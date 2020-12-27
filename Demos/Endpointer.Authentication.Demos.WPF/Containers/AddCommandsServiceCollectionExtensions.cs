using Endpointer.Demos.WPF.Commands;
using Endpointer.Demos.WPF.Commands.Authentication;
using Endpointer.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Demos.WPF.Containers
{
    public static class AddCommandsServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddSingleton<RefreshCommand>();
            services.AddSingleton<LogoutCommand>();
            services.AddSingleton<LogoutEverywhereCommand>();
            services.AddSingleton<NavigateCommand<RegisterViewModel>>();
            services.AddSingleton<NavigateCommand<LoginViewModel>>();
            services.AddSingleton<NavigateCommand<AccountViewModel>>();

            return services;
        }
    }
}
