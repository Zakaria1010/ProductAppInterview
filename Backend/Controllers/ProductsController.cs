using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;
using System.Text.Json;

namespace ProductApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static string _filePath = "products.json";
        private static List<Product> _products = LoadProducts();
        //private static List<Product> _products = new List<Product>
        //{
        //    new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Description 1" },
        //    new Product { Id = 2, Name = "Product 2", Price = 20, Description = "Description 2" },
        //    new Product { Id = 3, Name = "Product 3", Price = 30, Description = "Description 3" }
        //};

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }

        // Modifier un produit
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            var existingProduct = _products.Find(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            SaveProducts(); // Save the updated list back to the file
            return NoContent();
        }

        // Load products from the file
        private static List<Product> LoadProducts()
        {
            if (System.IO.File.Exists(_filePath))
            {
                var json = System.IO.File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
            return new List<Product>();
        }

        // Save products to the file
        private static void SaveProducts()
        {
            var json = JsonSerializer.Serialize(_products);
            System.IO.File.WriteAllText(_filePath, json);
        }
    }
}
