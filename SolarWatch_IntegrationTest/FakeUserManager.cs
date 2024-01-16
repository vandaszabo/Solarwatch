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
        return Task.FromResult(password == "correct_password");
    }

    public override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
    {
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}