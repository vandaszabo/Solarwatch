using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Repository;


namespace SolarWatch_IntegrationTest
{
    public class SolarWatchFactory : WebApplicationFactory<Program>
    {
        public SolarWatchContext _solarWatchContext { get; }
        public ISolarWatchRepository _solarWatchRepository;
        public ICityRepository _cityRepository;

        public SolarWatchFactory()
        {
            _solarWatchContext = new SolarWatchContext(new DbContextOptions<SolarWatchContext>());
            _solarWatchRepository = new SolarWatchRepository(_solarWatchContext);
            _cityRepository = new CityRepository(_solarWatchContext);

        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                var userStoreMock = new Mock<IUserStore<IdentityUser>>();
                var fakeUserManager = new FakeUserManager(userStoreMock.Object);

                // Register services
                services.AddDbContext<SolarWatchContext>(options =>
                {
                    options.UseInMemoryDatabase("SolarWatchTestDb");
                });
                services.AddTransient<ISolarWatchRepository, SolarWatchRepository>();
                services.AddTransient<ICityRepository, CityRepository>();
                
                services.AddTransient<IUserStore<IdentityUser>>(provider => userStoreMock.Object);
                services.AddSingleton<RoleManager<IdentityRole>, FakeRoleManager>();
                services.AddSingleton<UserManager<IdentityUser>>(fakeUserManager);
            });
            builder.UseEnvironment("Testing");
        }
    }


}
