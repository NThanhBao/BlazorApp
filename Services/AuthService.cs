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

        // Phương thức đăng ký người dùng
        public async Task RegisterAsync(string username, string password, string email, string phoneNumber)
        {
            // Kiểm tra nếu tên người dùng đã tồn tại
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
                throw new Exception("Username already exists.");

            // Mã hóa mật khẩu
            var passwordHash = HashPassword(password);

            // Tạo người dùng mới
            var newUser = new Users
            {
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                PhoneNumber = phoneNumber,
                DateCreated = DateTime.UtcNow,
                Role = UserRole.USER // Mặc định là USER
            };

            // Lưu vào cơ sở dữ liệu
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        // Phương thức đăng nhập
        public async Task<bool> LoginAsync(string username, string password)
        {
            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return false; // Nếu không tìm thấy người dùng, trả về false

            // Kiểm tra mật khẩu đã nhập với mật khẩu trong cơ sở dữ liệu
            return VerifyPassword(password, user.PasswordHash); // Sử dụng VerifyPassword
        }

        // Hàm mã hóa mật khẩu
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Hàm xác minh mật khẩu
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
