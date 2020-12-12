using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Demos.WPF.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddEndpointerAuthenticationClient();

                    services.AddSingleton<NavigationStore>();

                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            _host.Start();

            IServiceProvider services = _host.Services;

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
