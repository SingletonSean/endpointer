using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Accounts.API.Models
{
    /// <summary>
    /// Model for additional Endpointer accounts options.
    /// </summary>
    public class EndpointerAccountsOptions
    {
        public bool UseDatabase { get; set; }
        public Action<IServiceCollection> AddDbContext { get; set; }
        public Action<IServiceCollection> AddDbAccountRepository { get; set; }
    }
}
