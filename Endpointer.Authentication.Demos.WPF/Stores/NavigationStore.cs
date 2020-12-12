using Endpointer.Authentication.Demos.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnNavigationChanged();
            }
        }

        public event Action NavigationChanged;

        private void OnNavigationChanged()
        {
            NavigationChanged?.Invoke();
        }
    }
}
