using Microsoft.AspNetCore.Mvc.Testing;
using ProductApp.Models;
using System.Net;
using System.Text.Json;

namespace Backend.Tests
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(); // This starts the test server
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Act: Envoyer une requête GET pour récupérer les produits
            var response = await _client.GetAsync("/api/products");

            // Assert: Vérifier que le statut de la réponse est OK
            response.EnsureSuccessStatusCode();

            // Désérialisation de la réponse JSON en une liste de produits
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Vérifier que nous avons reçu des produits
            Assert.NotEmpty(products);
        }

        // Test : Vérifie que l'API retourne un produit modifié (admin)
        [Fact]
        public async Task UpdateProduct_ReturnsNoContentResult_WhenProductIsUpdated()
        {
            // Arrange: Créer un produit à mettre à jour
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 15, Description = "Updated Description" };

            // Convertir le produit en JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(updatedProduct),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // Act: Envoyer une requête PUT pour mettre à jour un produit existant
            var response = await _client.PutAsync("/api/products/1", jsonContent);

            // Assert: Vérifier que le code de statut est 204 (NoContent)
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        // Test : Vérifie que l'API retourne un code 404 si un produit n'existe pas pour la mise à jour
        [Fact]
        public async Task UpdateProduct_ReturnsNotFoundResult_WhenProductDoesNotExist()
        {
            // Arrange: Créer un produit à mettre à jour avec un ID inexistant
            var updatedProduct = new Product { Id = 999, Name = "Non-Existing Product", Price = 15, Description = "Non-Existing Description" };

            // Convertir le produit en JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(updatedProduct),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // Act: Envoyer une requête PUT pour tenter de mettre à jour un produit inexistant
            var response = await _client.PutAsync("/api/products/999", jsonContent);

            // Assert: Vérifier que le code de statut est 404 (NotFound)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
