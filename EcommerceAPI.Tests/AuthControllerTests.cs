using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AuthControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            // Create in-memory database with unique name per test instance
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new ApplicationDbContext(options);

            // Create test configuration
            var inMemorySettings = new Dictionary<string, string?> {
                { "JwtSettings:Secret", "TEST_SECRET_KEY_WITH_AT_LEAST_32_CHARACTERS_FOR_TESTING" },
                { "JwtSettings:Issuer", "EcommerceAPI" },
                { "JwtSettings:Audience", "EcommerceFrontend" },
                { "JwtSettings:ExpirationInHours", "24" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new AuthController(_context, _configuration);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            // Arrange
            var request = new RegisterRequest 
            { 
                Email = "test@example.com", 
                Username = "testuser", 
                Password = "Password123" 
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Single(_context.Users);
            
            var user = _context.Users.First();
            Assert.Equal(request.Email, user.Email);
            Assert.Equal(request.Username, user.Username);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            _context.Users.Add(new User 
            { 
                Email = "existing@example.com", 
                Username = "existing", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123"),
                Role = EcommerceAPI.Core.Enums.Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            var request = new RegisterRequest 
            { 
                Email = "existing@example.com", 
                Username = "newuser", 
                Password = "Password123" 
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var password = "Password123";
            _context.Users.Add(new User 
            { 
                Email = "user@example.com", 
                Username = "user", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = EcommerceAPI.Core.Enums.Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            var request = new LoginRequest 
            { 
                Email = "user@example.com", 
                Password = password 
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            _context.Users.Add(new User 
            { 
                Email = "user@example.com", 
                Username = "user", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                Role = EcommerceAPI.Core.Enums.Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            var request = new LoginRequest 
            { 
                Email = "user@example.com", 
                Password = "WrongPassword" 
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}