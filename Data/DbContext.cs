using BlazorApp_Auth.Models;
using Microsoft.EntityFrameworkCore;

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

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Products
                    {
                        Name = "Sản phẩm A",
                        Price = 100.00m,
                        Description = "Mô tả sản phẩm A",
                        Quantity = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Products
                    {
                        Name = "Sản phẩm B",
                        Price = 200.00m,
                        Description = "Mô tả sản phẩm B",
                        Quantity = 20,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Products
                    {
                        Name = "Sản phẩm C",
                        Price = 300.00m,
                        Description = "Mô tả sản phẩm C",
                        Quantity = 30,
                        CreatedAt = DateTime.UtcNow
                    }
                );

                context.SaveChanges();
            }
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
