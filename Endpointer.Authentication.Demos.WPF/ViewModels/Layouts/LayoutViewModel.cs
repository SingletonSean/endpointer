using Endpointer.Authentication.Demos.WPF.Commands;
using System.Windows.Input;

namespace Endpointer.Authentication.Demos.WPF.ViewModels.Layouts
{
    public delegate LayoutViewModel CreateLayoutViewModel(ViewModelBase viewModel);

    public class LayoutViewModel : ViewModelBase
    {
        public ICommand ShowRegisterCommand { get; }
        public ICommand ShowLoginCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }

        public ViewModelBase CurrentViewModel { get; }

        public LayoutViewModel(ViewModelBase currentViewModel,
            CreateCommand<LayoutViewModel> createShowRegisterCommand,
            CreateCommand<LayoutViewModel> createShowLoginCommand,
            CreateCommand<LayoutViewModel> refreshCommand,
            CreateCommand<LayoutViewModel> logoutCommand)
        {
            CurrentViewModel = currentViewModel;

            ShowRegisterCommand = createShowRegisterCommand(this);
            ShowLoginCommand = createShowLoginCommand(this);
            RefreshCommand = refreshCommand(this);
            LogoutCommand = logoutCommand(this);
        }
    }
}
