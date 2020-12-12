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
                await _logoutService.Logout($"Bearer {_tokenStore.AccessToken}");

                MessageBox.Show("Successfully logged out.", "Success");
            }
            catch (ApiException ex)
            {
                MessageBox.Show($"Logout failed. (Status Code: {ex.StatusCode})", "Error");
            }
        }
    }
}
