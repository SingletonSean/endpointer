using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract void Execute(object parameter);

        public virtual bool CanExecute(object parameter) => true;

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
