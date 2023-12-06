using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IEnumerable<IdentityUser>> GetAll()
    {
        var users = await _userManager.Users.ToListAsync();
        return users;
    }

    public async Task<string?> DeleteUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        
        if (user == null)
        {
            return null;
        }

        await _userManager.DeleteAsync(user);
        
        return user.UserName;
    }
    
    public async Task<IList<string>> GetRoles(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return new List<string>();
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        return roles;
    }
    
    public async Task<string?> SetRole(string userName, string role)
    {
        var user = await _userManager.FindByNameAsync(userName);
        
        if (user == null)
        {
            return null;
        }
        
        var existingRoles = await _userManager.GetRolesAsync(user);
        if (existingRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
        }
        await _userManager.AddToRoleAsync(user, role);

        return user.UserName;
    }
}