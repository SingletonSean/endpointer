using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Accounts.API.Models
{
    public class EndpointerAccountsOptions
    {
        public bool UseDatabase { get; set; }
        public Action<IServiceCollection> AddDbContext { get; set; }
    }
}
