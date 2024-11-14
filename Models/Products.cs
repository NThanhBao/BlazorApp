using System.ComponentModel.DataAnnotations;

namespace BlazorApp_Auth.Models
{
    public class Products
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; } 
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
