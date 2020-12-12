using Endpointer.Authentication.Demos.WPF.Commands;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Containers
{
    public static class AddCommandsServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddSingleton<RefreshCommand>();
            services.AddSingleton<LogoutCommand>();
            services.AddSingleton<NavigateCommand<RegisterViewModel>>();
            services.AddSingleton<NavigateCommand<LoginViewModel>>();

            return services;
        }
    }
}
