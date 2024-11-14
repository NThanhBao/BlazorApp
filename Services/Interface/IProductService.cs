using BlazorApp_Auth.Models;

namespace BlazorApp_Auth.Services
{
    public interface IProductService
    {
        Task<List<Products>> GetAllProductsAsync();  
        Task<Products> GetProductByIdAsync(int id);  
        Task AddProductAsync(Products product);
        Task EditProductAsync(Products product);   
        Task DeleteProductAsync(int id);             
    }
}
