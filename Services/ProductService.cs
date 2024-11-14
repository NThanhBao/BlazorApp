using BlazorApp_Auth.Data;
using BlazorApp_Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp_Auth.Services
{
    public class ProductService : IProductService
    {
        private readonly UserAuthDbContext _context;

        public ProductService(UserAuthDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả sản phẩm
        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Lấy sản phẩm theo ID
        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm sản phẩm mới
        public async Task AddProductAsync(Products product)
        {
            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Sửa thông tin sản phẩm
        public async Task EditProductAsync(Products product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Quantity = product.Quantity;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        // Xóa sản phẩm
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}
