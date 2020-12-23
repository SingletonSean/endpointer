using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Authentication.Demos.WPF.Stores;
using Refit;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class LogoutCommand : AsyncCommandBase
    {
        private readonly TokenStore _tokenStore;
        private readonly ILogoutService _logoutService;

        public LogoutCommand(TokenStore tokenStore, ILogoutService logoutService)
        {
            _tokenStore = tokenStore;
            _logoutService = logoutService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _logoutService.Logout();

                MessageBox.Show("Successfully logged out.", "Success");
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show($"Logout failed. Must be logged in to logout.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Logout failed. Not sure why...", "Error");
            }
        }
    }
}
