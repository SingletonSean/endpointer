using Endpointer.Authentication.Client.Services.VerifyEmail;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Demos.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands.Authentication
{
    public class VerifyEmailCommand : AsyncCommandBase
    {
        private readonly VerifyEmailViewModel _viewModel;
        private readonly IVerifyEmailService _verifyEmailService;

        public VerifyEmailCommand(VerifyEmailViewModel viewModel, IVerifyEmailService verifyEmailService)
        {
            _viewModel = viewModel;
            _verifyEmailService = verifyEmailService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            VerifyEmailRequest request = new VerifyEmailRequest()
            {
                VerifyToken = _viewModel.Token
            };

            try
            {
                await _verifyEmailService.VerifyEmail(request);

                MessageBox.Show("Successfully verified email.", "Success");
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show($"Verification failed. Invalid token.", "Error");
            }
            catch (ValidationFailedException)
            {
                MessageBox.Show($"Verification failed. Invalid request state.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Verification failed. Not sure why...", "Error");
            }
        }
    }
}
