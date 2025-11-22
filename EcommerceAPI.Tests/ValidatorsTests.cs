using EcommerceAPI.DTOs.Auth;
using EcommerceAPI.DTOs.Products;
using EcommerceAPI.Validators;
using FluentValidation.Results;
using Xunit;

namespace EcommerceAPI.Tests
{
    public class ValidatorsTests
    {
        [Fact]
        public void RegisterValidator_InvalidEmail_ReturnsError()
        {
            var validator = new RegisterRequestValidator();
            var request = new RegisterRequest { Email = "invalid", Username = "u", Password = "123456" };
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public void CreateProductValidator_InvalidPrice_ReturnsError()
        {
            var validator = new CreateProductRequestValidator();
            var request = new CreateProductRequest { Name = "p", Price = 0, Stock = 1 };
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Price");
        }
    }
}