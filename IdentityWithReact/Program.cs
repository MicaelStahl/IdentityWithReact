using System;
using DataAccessLibrary.Database;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityWithReact
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                //
                // Summary:
                // For readability:
                //  var context = services.GetRequiredService<ReactDbContext>();
                //  var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                //  var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                //  DbInitializer.Initializer(context, _userManager, _roleManager);
                DbInitializer.Initializer(
                    services.GetRequiredService<ReactDbContext>(),
                    services.GetRequiredService<UserManager<AppUser>>(),
                    services.GetRequiredService<RoleManager<IdentityRole>>());
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured while seeding the database.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}