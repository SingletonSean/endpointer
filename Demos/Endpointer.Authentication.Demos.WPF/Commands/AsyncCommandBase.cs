using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Endpointer.Demos.WPF.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isExecuting;

        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            private set
            {
                _isExecuting = value;
                OnCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
         
            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
        protected virtual void OnException(Exception ex) { }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
