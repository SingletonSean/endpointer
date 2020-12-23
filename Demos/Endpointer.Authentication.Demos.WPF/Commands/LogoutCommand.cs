using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Authentication.Demos.WPF.Stores;
using Refit;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class LogoutCommand : AsyncCommandBase
    {
        private readonly TokenStore _tokenStore;
        private readonly IAPILogoutService _logoutService;

        public LogoutCommand(TokenStore tokenStore, IAPILogoutService logoutService)
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
            catch (ApiException ex)
            {
                MessageBox.Show($"Logout failed. (Status Code: {ex.StatusCode})", "Error");
            }
        }
    }
}
