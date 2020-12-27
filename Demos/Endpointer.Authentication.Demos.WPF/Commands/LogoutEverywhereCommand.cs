using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Core.Client.Exceptions;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class LogoutEverywhereCommand : AsyncCommandBase
    {
        private readonly ILogoutEverywhereService _logoutEverywhereService;

        public LogoutEverywhereCommand(ILogoutEverywhereService logoutEverywhereService)
        {
            _logoutEverywhereService = logoutEverywhereService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _logoutEverywhereService.LogoutEverywhere();

                MessageBox.Show("Successfully logged out everywhere.", "Success");
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show($"Logout everywhere failed. Must be logged in to logout.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Logout everywhere failed. Not sure why...", "Error");
            }
        }
    }
}
