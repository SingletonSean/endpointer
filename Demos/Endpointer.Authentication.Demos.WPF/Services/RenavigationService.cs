using Endpointer.Demos.WPF.Stores;
using Endpointer.Demos.WPF.ViewModels;

namespace Endpointer.Demos.WPF.Services
{
    public class RenavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly CreateViewModel<TViewModel> _createViewModel;
        private readonly NavigationStore _navigationStore;

        public RenavigationService(CreateViewModel<TViewModel> createViewModel,
            NavigationStore navigationStore)
        {
            _createViewModel = createViewModel;
            _navigationStore = navigationStore;
        }

        public void Renavigate()
        {
            ViewModelBase viewModel = _createViewModel();
            _navigationStore.ShowInLayout(viewModel);
        }
    }
}
