using System.Windows.Input;

namespace Endpointer.Demos.WPF.Commands
{
    public delegate ICommand CreateCommand<TViewModel>(TViewModel viewmodel);
}
