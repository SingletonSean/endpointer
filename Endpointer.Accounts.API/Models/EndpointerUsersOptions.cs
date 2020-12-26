using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Users.API.Models
{
    public class EndpointerUsersOptions
    {
        public bool UseDatabase { get; set; }
        public Action<IServiceCollection> AddDbContext { get; set; }
    }
}
