using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch_IntegrationTest
{
    public class SolarWatchFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                // Add user secrets configuration source
                config.AddUserSecrets<Program>();
            });

            builder.ConfigureServices((context, services) =>
            {
                // Retrieve connection string from user secrets
                var connectionString = context.Configuration["ConnectionString"];

                // Replace YourDbContext1 and YourDbContext2 with the actual names of your DbContext classes
                var dbContext1Descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SolarWatchContext>)
                );

                if (dbContext1Descriptor != null)
                {
                    services.Remove(dbContext1Descriptor);
                }

                services.AddDbContext<SolarWatchContext>(options =>
                {
                    options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                });

                var dbContext2Descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<UsersContext>)
                );

                if (dbContext2Descriptor != null)
                {
                    services.Remove(dbContext2Descriptor);
                }
                
                services.AddDbContext<UsersContext>(options =>
                {
                    options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                });
            });
        }
    }
}
