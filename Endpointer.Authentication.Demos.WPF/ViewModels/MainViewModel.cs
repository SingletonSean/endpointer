using Endpointer.Authentication.Demos.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.NavigationChanged += NavigationStore_NavigationChanged;
        }

        private void NavigationStore_NavigationChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
