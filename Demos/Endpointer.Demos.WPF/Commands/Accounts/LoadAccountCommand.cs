using Endpointer.Accounts.Client.Exceptions;
using Endpointer.Accounts.Client.Services.Accounts;
using Endpointer.Accounts.Core.Models.Responses;
using Endpointer.Core.Client.Exceptions;
using Endpointer.Demos.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Endpointer.Demos.WPF.Commands.Accounts
{
    public class LoadAccountCommand : AsyncCommandBase
    {
        private readonly AccountViewModel _viewModel;
        private readonly IAccountService _accountService;

        public LoadAccountCommand(AccountViewModel viewModel, IAccountService accountService)
        {
            _viewModel = viewModel;
            _accountService = accountService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            bool success = false;
            _viewModel.IsLoading = true;

            try
            {
                AccountResponse account = await _accountService.GetAccount();

                _viewModel.Email = account.Email;
                _viewModel.Username = account.Username;

                success = true;
            }
            catch(UnauthorizedException)
            {
                MessageBox.Show($"Load account failed. Must login to get account information.", "Error");
            }
            catch(AccountNotFoundException)
            {
                MessageBox.Show($"Load account failed. Unable to find user's account.", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show($"Load account failed. Not sure why...", "Error");
            }

            if(!success)
            {
                _viewModel.Email = "N/A";
                _viewModel.Username = "N/A";
            }

            _viewModel.IsLoading = false;
        }
    }
}
