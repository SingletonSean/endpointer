using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.Demos.Web.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Endpointer.Authentication.Demos.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.RunMigrations<CustomDbContext>();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
