using BlazorApp_Auth.Models;

public interface IAuthService
{
    Task RegisterAsync(string username, string password, string email, string phoneNumber);
    Task<(bool, UserRole)> LoginAsync(string username, string password);
}
