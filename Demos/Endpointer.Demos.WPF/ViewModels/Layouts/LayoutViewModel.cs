﻿using Endpointer.Demos.WPF.Commands;
using System.Windows.Input;

namespace Endpointer.Demos.WPF.ViewModels.Layouts
{
    public delegate LayoutViewModel CreateLayoutViewModel(ViewModelBase viewModel);

    public class LayoutViewModel : ViewModelBase
    {
        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowLoginCommand { get; }
        public ICommand ShowAccountCommand { get; }
        public ICommand ShowVerifyEmailCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand LogoutEverywhereCommand { get; }

        public ViewModelBase CurrentViewModel { get; }

        public LayoutViewModel(ViewModelBase currentViewModel,
            CreateCommand<LayoutViewModel> createShowRegisterCommand,
            CreateCommand<LayoutViewModel> createShowLoginCommand,
            CreateCommand<LayoutViewModel> createShowAccountCommand,
            CreateCommand<LayoutViewModel> createShowVerifyEmailCommand,
            CreateCommand<LayoutViewModel> refreshCommand,
            CreateCommand<LayoutViewModel> logoutCommand,
            CreateCommand<LayoutViewModel> logoutEverywhereCommand)
        {
            CurrentViewModel = currentViewModel;

            ShowRegisterCommand = createShowRegisterCommand(this);
            ShowLoginCommand = createShowLoginCommand(this);
            ShowAccountCommand = createShowAccountCommand(this);
            ShowVerifyEmailCommand = createShowVerifyEmailCommand(this);
            RefreshCommand = refreshCommand(this);
            LogoutCommand = logoutCommand(this);
            LogoutEverywhereCommand = logoutEverywhereCommand(this);
        }
    }
}
