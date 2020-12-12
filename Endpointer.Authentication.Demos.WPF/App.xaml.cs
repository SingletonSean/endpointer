using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Demos.WPF.Commands;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Endpointer.Authentication.Demos.WPF.ViewModels.Layouts;
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
                    string baseAddress = "https://localhost:5001/";
                    AuthenticationEndpointsConfiguration endpointConfiguration = new AuthenticationEndpointsConfiguration()
                    {
                        RegisterEndpoint = baseAddress + "register",
                        LoginEndpoint = baseAddress + "login",
                        RefreshEndpoint = baseAddress + "refresh",
                        LogoutEndpoint = baseAddress + "logout",
                    };
                    services.AddEndpointerAuthenticationClient(endpointConfiguration);

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<TokenStore>();

                    services.AddSingleton<NavigateCommand<RegisterViewModel>>();
                    services.AddSingleton<NavigateCommand<LoginViewModel>>();
                    services.AddSingleton<RefreshCommand>();
                    services.AddSingleton<LogoutCommand>();

                    services.AddSingleton<CreateViewModel<RegisterViewModel>>(s => () => s.GetRequiredService<RegisterViewModel>());
                    services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());

                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<CreateLayoutViewModel>(CreateLayoutViewModelFactory);
                    services.AddSingleton<RegisterViewModel>(CreateRegisterViewModel);
                    services.AddSingleton<LoginViewModel>(CreateLoginViewModel);

                    services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
                })
                .Build();
        }

        private LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel(vm => new LoginCommand(vm, 
                services.GetRequiredService<TokenStore>(),
                services.GetRequiredService<ILoginService>()));
        }

        private RegisterViewModel CreateRegisterViewModel(IServiceProvider services)
        {
            return new RegisterViewModel(vm => new RegisterCommand(vm, services.GetRequiredService<IRegisterService>()));
        }

        private CreateLayoutViewModel CreateLayoutViewModelFactory(IServiceProvider services)
        {
            return vm => new LayoutViewModel(vm, 
                (vm) => services.GetRequiredService<NavigateCommand<RegisterViewModel>>(),
                (vm) => services.GetRequiredService<NavigateCommand<LoginViewModel>>(), 
                (vm) => services.GetRequiredService<RefreshCommand>(),
                (vm) => services.GetRequiredService<LogoutCommand>());
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
