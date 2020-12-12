using Endpointer.Authentication.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Containers
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
