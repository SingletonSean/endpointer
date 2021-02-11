using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Accounts.API.Models
{
    /// <summary>
    /// Model for additional Endpointer accounts options.
    /// </summary>
    public class EndpointerAccountsOptions
    {
        public Action<IServiceCollection> AddDataSourceServices { get; set; }
    }
}
