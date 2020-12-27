using Endpointer.Demos.WPF.Stores;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands
{
    public class RefreshCommand : AsyncCommandBase
    {
        private readonly TokenStore _tokenStore;
        private readonly IRefreshService _refreshService;

        public RefreshCommand(TokenStore tokenStore, IRefreshService refreshService)
        {
            _tokenStore = tokenStore;
            _refreshService = refreshService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            RefreshRequest request = new RefreshRequest()
            {
                RefreshToken = await _tokenStore.GetRefreshToken()
            };

            try
            {
                AuthenticatedUserResponse response = await _refreshService.Refresh(request);
                await _tokenStore.SetTokens(response.AccessToken, response.RefreshToken, response.AccessTokenExpirationTime);

                MessageBox.Show("Successfully refreshed.", "Success");
            }
            catch (InvalidRefreshTokenException)
            {
                MessageBox.Show($"Refresh failed. Refresh token invalid/expired.", "Error");
            }
            catch (ValidationFailedException)
            {
                MessageBox.Show($"Refresh failed. Please provide a refresh token.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Refresh failed. Not sure why...", "Error");
            }
        }
    }
}
