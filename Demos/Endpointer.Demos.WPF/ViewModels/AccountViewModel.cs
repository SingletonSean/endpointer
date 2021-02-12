using Endpointer.Demos.WPF.Commands;
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

        private bool _isEmailVerified;
        public bool IsEmailVerified
        {
            get
            {
                return _isEmailVerified;
            }
            set
            {
                _isEmailVerified = value;
                OnPropertyChanged(nameof(IsEmailVerified));
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
        public ICommand SendEmailVerificationEmailCommand { get; }

        public AccountViewModel(
            CreateCommand<AccountViewModel> createLoadAccountCommand,
            CreateCommand<AccountViewModel> createSendEmailVerificationEmailCommand)
        {
            LoadAccountCommand = createLoadAccountCommand(this);
            SendEmailVerificationEmailCommand = createSendEmailVerificationEmailCommand(this);
        }

        public static AccountViewModel LoadViewModel(
            CreateCommand<AccountViewModel> createLoadAccountCommand, 
            CreateCommand<AccountViewModel> createSendEmailVerificationEmailCommand)
        {
            AccountViewModel viewModel = new AccountViewModel(createLoadAccountCommand, createSendEmailVerificationEmailCommand);

            viewModel.LoadAccountCommand.Execute(null);

            return viewModel;
        }
    }
}
