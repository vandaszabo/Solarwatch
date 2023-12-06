using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services;

public interface IUserService
{
    Task<IEnumerable<IdentityUser>> GetAll();
    Task<string?> DeleteUser(string userName);
    Task<IList<string>> GetRoles(string userName);
    Task<string?> SetRole(string username, string role);
}