using Endpointer.Accounts.Client.Services.Accounts;
using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Authentication.Client.Services.VerifyEmail;
using Endpointer.Demos.WPF.Commands;
using Endpointer.Demos.WPF.Commands.Accounts;
using Endpointer.Demos.WPF.Commands.Authentication;
using Endpointer.Demos.WPF.Services;
using Endpointer.Demos.WPF.Stores;
using Endpointer.Demos.WPF.ViewModels;
using Endpointer.Demos.WPF.ViewModels.Layouts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Demos.WPF.Containers
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
            services.AddSingleton<CreateViewModel<AccountViewModel>>(CreateAccountViewModel);
            services.AddSingleton<CreateViewModel<VerifyEmailViewModel>>(CreateVerifyEmailViewModel);

            return services;
        }

        private static CreateLayoutViewModel CreateLayoutViewModelFactory(IServiceProvider services)
        {
            return vm => new LayoutViewModel(vm,
                (vm) => services.GetRequiredService<NavigateCommand<RegisterViewModel>>(),
                (vm) => services.GetRequiredService<NavigateCommand<LoginViewModel>>(),
                (vm) => services.GetRequiredService<NavigateCommand<AccountViewModel>>(),
                (vm) => services.GetRequiredService<NavigateCommand<VerifyEmailViewModel>>(),
                (vm) => services.GetRequiredService<RefreshCommand>(),
                (vm) => services.GetRequiredService<LogoutCommand>(),
                (vm) => services.GetRequiredService<LogoutEverywhereCommand>());
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

        private static CreateViewModel<AccountViewModel> CreateAccountViewModel(IServiceProvider services)
        {
            return () => AccountViewModel.LoadViewModel(vm => new LoadAccountCommand(vm,
                services.GetRequiredService<IAccountService>()));
        }

        private static CreateViewModel<VerifyEmailViewModel> CreateVerifyEmailViewModel(IServiceProvider services)
        {
            return () => new VerifyEmailViewModel(vm => new VerifyEmailCommand(vm, 
                services.GetRequiredService<IVerifyEmailService>()));
        }
    }
}
