using System.Threading.Tasks;
using EcommerceAPI.Controllers;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.Auth;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EcommerceAPI.Tests
{
    public class AuthControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestsDb")
                .Options;
            return new ApplicationDbContext(options);
        }

        private IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
                { "JwtSettings:Secret", "TEST_SECRET_012345678901234567890123" },
                { "JwtSettings:Issuer", "EcommerceAPI" },
                { "JwtSettings:Audience", "EcommerceFrontend" },
                { "JwtSettings:ExpirationInHours", "24" }
            };
            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }

        [Fact]
        public async Task Register_Should_Create_User()
        {
            var context = GetInMemoryDbContext();
            var config = GetConfiguration();
            var controller = new AuthController(context, config);

            var request = new RegisterRequest { Email = "new@test.com", Username = "newuser", Password = "Password1" };
            var result = await controller.Register(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Single(context.Users);
        }

        [Fact]
        public void Login_Invalid_Returns_Unauthorized()
        {
            var context = GetInMemoryDbContext();
            context.Users.Add(new User { Email = "a@test.com", Username = "a", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"), Role = Core.Enums.Role.User, CreatedAt = System.DateTime.UtcNow, UpdatedAt = System.DateTime.UtcNow });
            context.SaveChanges();

            var config = GetConfiguration();
            var controller = new AuthController(context, config);

            var login = new LoginRequest { Email = "a@test.com", Password = "wrong" };
            var result = controller.Login(login) as UnauthorizedObjectResult;

            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
    }
}