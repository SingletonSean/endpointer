using Endpointer.Authentication.Demos.WPF.Containers;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        private IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder().ConfigureServices(services =>
            {
                services
                    .AddServices()
                    .AddStores()
                    .AddCommands()
                    .AddViewModels()
                    .AddViews();
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            IServiceProvider services = _host.Services;

            NavigationStore navigationStore = services.GetRequiredService<NavigationStore>();
            navigationStore.ShowInLayout(services.GetRequiredService<RegisterViewModel>());

            MainWindow = services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
