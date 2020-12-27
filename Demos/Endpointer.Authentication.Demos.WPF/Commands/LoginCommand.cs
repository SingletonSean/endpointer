using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Models.Responses;
using System;
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
                AuthenticatedUserResponse response = await _loginService.Login(request);
                await _tokenStore.SetTokens(response.AccessToken, response.RefreshToken, response.AccessTokenExpirationTime);
                
                MessageBox.Show("Successfully logged in.", "Success");
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show($"Login failed. Invalid credentials.", "Error");
            }
            catch (ValidationFailedException)
            {
                MessageBox.Show($"Login failed. Invalid request.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Login failed. Not sure why...", "Error");
            }
        }
    }
}
