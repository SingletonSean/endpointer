using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Endpointer.Core.API.Extensions
{
    public static class RunMigrationsHostExtension
    {
        /// <summary>
        /// Run migrations against a DbContext's database.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <param name="host">The application host.</param>
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
