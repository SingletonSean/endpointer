using Endpointer.Demos.WPF.Services;
using Endpointer.Demos.WPF.ViewModels;

namespace Endpointer.Demos.WPF.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly RenavigationService<TViewModel> _renavigationService;

        public NavigateCommand(RenavigationService<TViewModel> renavigationService)
        {
            _renavigationService = renavigationService;
        }

        public override void Execute(object parameter)
        {
            _renavigationService.Renavigate();
        }
    }
}
