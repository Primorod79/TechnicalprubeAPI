using System.Threading.Tasks;
using EcommerceAPI.Controllers;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.Products;
using EcommerceAPI.Models;
using EcommerceAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EcommerceAPI.Tests
{
    public class ProductsControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductsTestsDb")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Create_Product_As_Admin_Returns_Created()
        {
            var context = GetInMemoryDbContext();
            var repo = new ProductRepository(context);
            var controller = new ProductsController(repo);

            var request = new CreateProductRequest { Name = "Test Product", Price = 10.5m, Stock = 5 };

            // Simulate authorization by not enforcing [Authorize] here; repository will work
            var result = await controller.Create(request) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Single(context.Products);
        }

        [Fact]
        public async Task Get_Product_NotFound_Returns_NotFound()
        {
            var context = GetInMemoryDbContext();
            var repo = new ProductRepository(context);
            var controller = new ProductsController(repo);

            var result = await controller.Get(999) as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}