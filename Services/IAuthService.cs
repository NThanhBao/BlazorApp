public interface IAuthService
{
    Task RegisterAsync(string username, string password, string email, string phoneNumber);
    Task<bool> LoginAsync(string username, string password); // Phương thức đăng nhập
}
