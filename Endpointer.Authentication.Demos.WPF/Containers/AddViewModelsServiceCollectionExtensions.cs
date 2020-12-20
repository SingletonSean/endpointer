using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Demos.WPF.Commands;
using Endpointer.Authentication.Demos.WPF.Services;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Endpointer.Authentication.Demos.WPF.ViewModels.Layouts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.Demos.WPF.Containers
{
    public static class AddViewModelsServiceCollectionExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<CreateLayoutViewModel>(CreateLayoutViewModelFactory);
            services.AddSingleton<RegisterViewModel>(CreateRegisterViewModel);
            services.AddSingleton<LoginViewModel>(CreateLoginViewModel);

            services.AddSingleton<CreateViewModel<RegisterViewModel>>(s => () => s.GetRequiredService<RegisterViewModel>());
            services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());

            return services;
        }

        private static CreateLayoutViewModel CreateLayoutViewModelFactory(IServiceProvider services)
        {
            return vm => new LayoutViewModel(vm,
                (vm) => services.GetRequiredService<NavigateCommand<RegisterViewModel>>(),
                (vm) => services.GetRequiredService<NavigateCommand<LoginViewModel>>(),
                (vm) => services.GetRequiredService<RefreshCommand>(),
                (vm) => services.GetRequiredService<LogoutCommand>());
        }

        private static RegisterViewModel CreateRegisterViewModel(IServiceProvider services)
        {
            return new RegisterViewModel(vm => new RegisterCommand(vm,
                services.GetRequiredService<IRegisterService>(),
                services.GetRequiredService<RenavigationService<LoginViewModel>>()));
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        {
            return new LoginViewModel(vm => new LoginCommand(vm,
                services.GetRequiredService<TokenStore>(),
                services.GetRequiredService<ILoginService>()));
        }
    }
}
