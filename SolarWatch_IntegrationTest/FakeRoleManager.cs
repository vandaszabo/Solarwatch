using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace SolarWatch_IntegrationTest;

public class FakeRoleManager : RoleManager<IdentityRole>
{
    public FakeRoleManager(IRoleStore<IdentityRole> roleStore, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger)
        : base(roleStore, roleValidators, keyNormalizer, errors, logger)
    {
    }

    public override Task<IdentityResult> CreateAsync(IdentityRole role)
    {
        return Task.FromResult(IdentityResult.Success);
    }
    
}
