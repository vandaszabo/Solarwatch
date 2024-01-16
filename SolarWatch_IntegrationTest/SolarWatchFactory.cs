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
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                var userStoreMock = new Mock<IUserStore<IdentityUser>>();
                
                // Set up IUserEmailStore<IdentityUser> methods
                userStoreMock.As<IUserEmailStore<IdentityUser>>()
                    .Setup(x => x.FindByEmailAsync(It.IsAny<string>(), CancellationToken.None))
                    .ReturnsAsync((string email, CancellationToken token) =>
                    {
                        // Implement the logic to find a user by email
                        var user = new IdentityUser { Email = email };
                        return user;
                    });
                
                // Create an instance of FakeUserManager with the mock user store
                var fakeUserManager = new FakeUserManager(userStoreMock.Object);

                // Register services
                services.AddTransient<IUserStore<IdentityUser>>(provider => userStoreMock.Object);
                services.AddSingleton<RoleManager<IdentityRole>, FakeRoleManager>();

             
                services.AddSingleton<UserManager<IdentityUser>>(fakeUserManager);
            });
            builder.UseEnvironment("Testing");
        }
    }


}
