using Endpointer.Authentication.Demos.WPF.Commands;
using Endpointer.Authentication.Demos.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Endpointer.Authentication.Demos.WPF.ViewModels.Layouts
{
    public delegate LayoutViewModel CreateLayoutViewModel(ViewModelBase viewModel);

    public class LayoutViewModel : ViewModelBase
    {
        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowLoginCommand { get; }
        public ICommand ShowRefreshCommand { get; }
        public ICommand ShowLogoutCommand { get; }

        public ViewModelBase CurrentViewModel { get; }

        public LayoutViewModel(ViewModelBase currentViewModel,
            CreateCommand<LayoutViewModel> createShowRegisterCommand,
            CreateCommand<LayoutViewModel> createShowLoginCommand,
            CreateCommand<LayoutViewModel> createShowRefreshCommand,
            CreateCommand<LayoutViewModel> createShowLogoutCommand
            )
        {
            CurrentViewModel = currentViewModel;

            ShowRegisterCommand = createShowRegisterCommand(this);
            ShowLoginCommand = createShowLoginCommand(this);
            ShowRefreshCommand = createShowRefreshCommand(this);
            ShowLogoutCommand = createShowLogoutCommand(this);
        }
    }
}
