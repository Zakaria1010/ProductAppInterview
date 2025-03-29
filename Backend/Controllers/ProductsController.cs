using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Description 1" },
            new Product { Id = 2, Name = "Product 2", Price = 20, Description = "Description 2" },
            new Product { Id = 3, Name = "Product 3", Price = 30, Description = "Description 3" }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }

        // Modifier un produit
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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
            return NoContent();
        }
    }
}
