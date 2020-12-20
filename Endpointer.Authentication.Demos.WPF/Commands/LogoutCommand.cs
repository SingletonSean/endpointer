﻿using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Demos.WPF.Stores;
using Refit;
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
            catch (ApiException ex)
            {
                MessageBox.Show($"Logout failed. (Status Code: {ex.StatusCode})", "Error");
            }
        }
    }
}