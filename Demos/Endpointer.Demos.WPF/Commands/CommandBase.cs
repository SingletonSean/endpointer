﻿using System;
using System.Windows.Input;

namespace Endpointer.Demos.WPF.Commands
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
