using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Endpointer.Authentication.API.Extensions
{
    public static class RunMigrationsHostExtension
    {
        public static void RunMigrations<TDbContext>(this IHost host) where TDbContext : DbContext
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                using (TDbContext context = scope.ServiceProvider.GetService<TDbContext>())
                {
                    if(context != null)
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }
    }
}
