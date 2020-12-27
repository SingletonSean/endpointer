using Endpointer.Demos.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Endpointer.Demos.WPF.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public ICommand LoadAccountCommand { get; }

        public AccountViewModel(CreateCommand<AccountViewModel> createLoadAccountCommand)
        {
            LoadAccountCommand = createLoadAccountCommand(this);
        }

        public static AccountViewModel LoadViewModel(CreateCommand<AccountViewModel> createLoadAccountCommand)
        {
            AccountViewModel viewModel = new AccountViewModel(createLoadAccountCommand);

            viewModel.LoadAccountCommand.Execute(null);

            return viewModel;
        }
    }
}
