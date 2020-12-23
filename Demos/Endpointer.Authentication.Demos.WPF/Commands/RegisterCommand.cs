using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Demos.WPF.Services;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _viewModel;
        private readonly IAPIRegisterService _registerService;
        private readonly RenavigationService<LoginViewModel> _loginRenavigationService;

        public RegisterCommand(RegisterViewModel viewModel,
            IAPIRegisterService registerService, 
            RenavigationService<LoginViewModel> loginRenavigationService)
        {
            _viewModel = viewModel;
            _registerService = registerService;
            _loginRenavigationService = loginRenavigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            RegisterRequest request = new RegisterRequest()
            {
                Email = _viewModel.Email,
                Username = _viewModel.Username,
                Password = _viewModel.Password,
                ConfirmPassword = _viewModel.ConfirmPassword,
            };

            try
            {
                await _registerService.Register(request);
                MessageBox.Show("Successfully registered.", "Success");

                _loginRenavigationService.Renavigate();
            }
            catch (ApiException ex)
            {
                ErrorResponse response = await ex.GetContentAsAsync<ErrorResponse>();
                MessageBox.Show($"Register failed. (Error Code: {response.Errors.FirstOrDefault()?.Code})", "Error");
            }
        }
    }
}
