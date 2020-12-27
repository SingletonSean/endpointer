using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Client.Services.Register;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Demos.WPF.Services;
using Endpointer.Demos.WPF.ViewModels;
using Endpointer.Core.Client.Exceptions;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands.Authentication
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _viewModel;
        private readonly IRegisterService _registerService;
        private readonly RenavigationService<LoginViewModel> _loginRenavigationService;

        public RegisterCommand(RegisterViewModel viewModel,
            IRegisterService registerService, 
            RenavigationService<LoginViewModel> loginRenavigationService)
        {
            _viewModel = viewModel;
            _registerService = registerService;
            _loginRenavigationService = loginRenavigationService;
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

                _loginRenavigationService.Renavigate();
            }
            catch (ConfirmPasswordException)
            {
                MessageBox.Show($"Register failed. Password must match confirm password.", "Error");
            }
            catch (EmailAlreadyExistsException ex)
            {
                MessageBox.Show($"Register failed. Email {ex.Email} is already taken.", "Error");
            }
            catch (UsernameAlreadyExistsException ex)
            {
                MessageBox.Show($"Register failed. Username {ex.Username} is already taken.", "Error");
            }
            catch (ValidationFailedException)
            {
                MessageBox.Show($"Register failed. Invalid register request.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Register failed. Not sure why...", "Error");
            }
        }
    }
}
