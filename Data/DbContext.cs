using BlazorApp_Auth.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace BlazorApp_Auth.Data
{
    public class UserAuthDbContext : DbContext
    {
        public UserAuthDbContext(DbContextOptions<UserAuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));
        }

        public static void SeedData(UserAuthDbContext context)
        {
            // Kiểm tra xem có dữ liệu trong bảng Users chưa
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new Users
                    {
                        Username = "admin1",
                        PasswordHash = HashPassword("admin"),
                        Email = "admin1@example.com",
                        PhoneNumber = "0123456789",
                        DateCreated = DateTime.Now,
                        Role = UserRole.ADMIN
                    },
                    new Users
                    {
                        Username = "admin2",
                        PasswordHash = HashPassword("admin"),
                        Email = "admin2@example.com",
                        PhoneNumber = "0123456789",
                        DateCreated = DateTime.Now,
                        Role = UserRole.ADMIN
                    }
                );

                context.SaveChanges();
            }
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
