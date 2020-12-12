using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Services
{
    public class RenavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly CreateViewModel<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public RenavigationService(CreateViewModel<TViewModel> createViewModel,
            NavigationStore navigationStore)
        {
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }

        public void Renavigate()
        {
            ViewModelBase viewModel = _createViewModel();
            _navigationStore.ShowInLayout(viewModel);
        }
    }
}
