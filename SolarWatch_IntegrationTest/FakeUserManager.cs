using Microsoft.AspNetCore.Identity;
using Moq;

namespace SolarWatch_IntegrationTest;

public class FakeUserManager : UserManager<IdentityUser>
{
    public FakeUserManager(IUserStore<IdentityUser> userStore)
        : base(userStore, null, null, null, null, null, null, null, null)
    {
    }

    public override Task<bool> CheckPasswordAsync(IdentityUser user, string password)
    {
        // Simulate the logic to check the user's password
        return Task.FromResult(password == "password1");
    }

    public override Task<IdentityUser> FindByEmailAsync(string email)
    {
        // Create a new instance of IdentityUser with the specified email
        var user = new IdentityUser { Email = email, UserName = "user1"};
        return Task.FromResult(user);
    }

    public override Task<IdentityUser> FindByNameAsync(string username)
    {
        var user = new IdentityUser { Email = "user1@email.com", UserName = username};
        return Task.FromResult(user);
    }
    
    public override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IList<string>> GetRolesAsync(IdentityUser user)
    {
        var roles = new List<string> { "User" };
        return Task.FromResult<IList<string>>(roles);
    }
    
}