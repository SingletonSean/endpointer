using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Demos.WPF.Stores;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands
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
                string refreshToken = await _tokenStore.GetRefreshToken();
                await _logoutService.Logout(refreshToken);

                _tokenStore.ClearAccessToken();

                MessageBox.Show("Successfully logged out.", "Success");
            }
            catch (Exception)
            {
                MessageBox.Show($"Logout failed. Not sure why...", "Error");
            }
        }
    }
}
