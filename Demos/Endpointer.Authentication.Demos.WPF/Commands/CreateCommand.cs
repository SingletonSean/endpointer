using System.Windows.Input;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public delegate ICommand CreateCommand<TViewModel>(TViewModel viewmodel);
}
