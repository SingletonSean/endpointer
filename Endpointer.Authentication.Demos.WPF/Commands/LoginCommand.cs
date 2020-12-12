using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Refit;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly TokenStore _tokenStore;
        private readonly ILoginService _loginService;

        public LoginCommand(LoginViewModel viewModel, TokenStore tokenStore, ILoginService loginService)
        {
            _viewModel = viewModel;
            _tokenStore = tokenStore;
            _loginService = loginService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            LoginRequest request = new LoginRequest()
            {
                Username = _viewModel.Username,
                Password = _viewModel.Password,
            };

            try
            {
                SuccessResponse<AuthenticatedUserResponse> response = await _loginService.Login(request);
                AuthenticatedUserResponse data = response.Data;

                if (data != null)
                {
                    _tokenStore.AccessToken = data.AccessToken;
                    _tokenStore.RefreshToken = data.RefreshToken;
                    MessageBox.Show("Successfully logged in.", "Success");
                }
            }
            catch (ApiException ex)
            {
                ErrorResponse response = await ex.GetContentAsAsync<ErrorResponse>();
                MessageBox.Show($"Login failed. (Error Code: {response.Errors.FirstOrDefault()?.Code})", "Error");
            }
        }
    }
}
