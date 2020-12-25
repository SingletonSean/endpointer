using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Endpointer.Authentication.API.Contexts
{
    public class DefaultAuthenticationDbContext : AuthenticationDbContext<User>
    {
        public DefaultAuthenticationDbContext(DbContextOptions<DefaultAuthenticationDbContext> options) : base(options) { }
    }
}
