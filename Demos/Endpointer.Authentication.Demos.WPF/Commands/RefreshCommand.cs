﻿using Endpointer.Authentication.Demos.WPF.Stores;
using Endpointer.Core.Client.Services;
using Endpointer.Core.Client.Services.Refresh;
using Endpointer.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using Refit;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class RefreshCommand : AsyncCommandBase
    {
        private readonly TokenStore _tokenStore;
        private readonly IAPIRefreshService _refreshService;

        public RefreshCommand(TokenStore tokenStore, IAPIRefreshService refreshService)
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
                SuccessResponse<AuthenticatedUserResponse> response = await _refreshService.Refresh(request);
                AuthenticatedUserResponse data = response.Data;

                if(data != null)
                {
                    await _tokenStore.SetTokens(data.AccessToken, data.RefreshToken, data.AccessTokenExpirationTime);
                    MessageBox.Show("Successfully refreshed.", "Success");
                }
            }
            catch (ApiException ex)
            {
                ErrorResponse response = await ex.GetContentAsAsync<ErrorResponse>();
                MessageBox.Show($"Refresh failed. (Error Code: {response.Errors.FirstOrDefault()?.Code})", "Error");
            }
        }
    }
}
