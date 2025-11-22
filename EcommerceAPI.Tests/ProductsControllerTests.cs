using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Controllers;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.Products;
using EcommerceAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceAPI.Tests
{
    public class ProductsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
            _controller = new ProductsController(_repository);
        }

        [Fact]
        public async Task GetAll_ReturnsProducts()
        {
            // Arrange
            _context.Products.Add(new EcommerceAPI.Models.Product
            {
                Name = "Test Product",
                Price = 10.99m,
                Stock = 5,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var product = new EcommerceAPI.Models.Product
            {
                Name = "Test Product",
                Price = 15.50m,
                Stock = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Get(product.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_NonExistingProduct_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Get(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Create_ValidProduct_ReturnsCreated()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Description = "Test Description",
                Price = 25.00m,
                Stock = 20
            };

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Single(_context.Products);
        }

        [Fact]
        public async Task Delete_ExistingProduct_ReturnsOk()
        {
            // Arrange
            var product = new EcommerceAPI.Models.Product
            {
                Name = "Product to Delete",
                Price = 5.00m,
                Stock = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Empty(_context.Products);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}