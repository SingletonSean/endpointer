﻿using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using Endpointer.Authentication.Demos.WPF.ViewModels;
using Refit;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Authentication.Demos.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _viewModel;
        private readonly IRegisterService _registerService;

        public RegisterCommand(RegisterViewModel viewModel, IRegisterService registerService)
        {
            _viewModel = viewModel;
            _registerService = registerService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            RegisterRequest request = new RegisterRequest()
            {
                Email = _viewModel.Email,
                Username = _viewModel.Username,
                Password = _viewModel.Password,
                ConfirmPassword = _viewModel.ConfirmPassword,
            };

            try
            {
                await _registerService.Register(request);
                MessageBox.Show("Successfully registered.", "Success");
            }
            catch (ApiException ex)
            {
                ErrorResponse response = await ex.GetContentAsAsync<ErrorResponse>();
                MessageBox.Show($"Register failed. (Error Code: {response.Errors.FirstOrDefault()?.Code})", "Error");
            }
        }
    }
}
