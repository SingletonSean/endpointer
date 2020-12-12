using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using System;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly CreateViewModel<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public NavigateCommand(CreateViewModel<TViewModel> createViewModel,
            NavigationStore navigationStore)
        {
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ViewModelBase viewModel = _createViewModel();
            _navigationStore.ShowInLayout(viewModel);
        }
    }
}
