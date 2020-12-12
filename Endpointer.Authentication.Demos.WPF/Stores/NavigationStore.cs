﻿using Endpointer.Authentication.Demos.WPF.ViewModels;
using Endpointer.Authentication.Demos.WPF.ViewModels.Layouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endpointer.Authentication.Demos.WPF.Stores
{
    public class NavigationStore
    {
        private readonly CreateLayoutViewModel _createLayoutViewModel;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            private set
            {
                _currentViewModel = value;
                OnNavigationChanged();
            }
        }

        public event Action NavigationChanged;

        public NavigationStore(CreateLayoutViewModel createLayoutViewModel)
        {
            _createLayoutViewModel = createLayoutViewModel;
        }

        public void ShowInLayout(ViewModelBase viewModel)
        {
            CurrentViewModel = _createLayoutViewModel(viewModel);
        }

        private void OnNavigationChanged()
        {
            NavigationChanged?.Invoke();
        }
    }
}
