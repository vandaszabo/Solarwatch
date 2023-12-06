using SolarWatch.Services.Authentication;

namespace SolarWatch.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role);
    Task<AuthResult> LoginAsync(string username, string password);
}