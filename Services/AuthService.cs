using BlazorApp_Auth.Models;
using BlazorApp_Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp_Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserAuthDbContext _context;

        public AuthService(UserAuthDbContext context)
        {
            _context = context;
        }

        // Phương thức đăng ký
        public async Task RegisterAsync(string username, string password, string email, string phoneNumber)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
                throw new Exception("Username already exists.");

            var passwordHash = HashPassword(password);

            var newUser = new Users
            {
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                PhoneNumber = phoneNumber,
                DateCreated = DateTime.UtcNow,
                Role = UserRole.USER 
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        // Phương thức đăng nhập
        public async Task<(bool, UserRole)> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return (true, user.Role); 
            }

            return (false, UserRole.USER); 
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
