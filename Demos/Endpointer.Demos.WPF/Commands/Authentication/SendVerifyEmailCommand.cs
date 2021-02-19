using Endpointer.Authentication.Client.Services.SendVerifyEmail;
using Endpointer.Core.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands.Authentication
{
    public class SendVerifyEmailCommand : AsyncCommandBase
    {
        private readonly ISendVerifyEmailService _sendVerifyEmailService;

        public SendVerifyEmailCommand(ISendVerifyEmailService sendVerifyEmailService)
        {
            _sendVerifyEmailService = sendVerifyEmailService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _sendVerifyEmailService.SendVerifyEmail();

                MessageBox.Show($"Successfully sent email verification email.", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show($"Send verify email failed. Must be logged in to send verification email.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show($"Send verify email failed. Not sure why...", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
