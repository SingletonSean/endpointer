using Endpointer.Demos.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Endpointer.Demos.WPF.ViewModels
{
    public class VerifyEmailViewModel : ViewModelBase
    {
        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
                OnPropertyChanged(nameof(Token));
            }
        }

        public ICommand VerifyEmailCommand { get; }

        public VerifyEmailViewModel(CreateCommand<VerifyEmailViewModel> createVerifyEmailCommand)
        {
            VerifyEmailCommand = createVerifyEmailCommand(this);
        }
    }
}
