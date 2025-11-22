using AutoMapper;
using EcommerceAPI.Core.Interfaces;
using EcommerceAPI.DTOs.Products;
using EcommerceAPI.Helpers;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] int? categoryId = null)
        {
            var result = await _repo.GetAllAsync(page, pageSize, search, categoryId);
            return Ok(ApiResponse<PaginatedResult<Product>>.SuccessResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound(ApiResponse<object>.Failure("Product not found", null, 404));
            return Ok(ApiResponse<Product>.SuccessResponse(product));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var created = await _repo.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, ApiResponse<Product>.SuccessResponse(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound(ApiResponse<object>.Failure("Product not found", null, 404));

            if (request.Name != null) existing.Name = request.Name;
            if (request.Description != null) existing.Description = request.Description;
            if (request.Price.HasValue) existing.Price = request.Price.Value;
            if (request.Stock.HasValue) existing.Stock = request.Stock.Value;
            if (request.ImageUrl != null) existing.ImageUrl = request.ImageUrl;
            if (request.CategoryId.HasValue) existing.CategoryId = request.CategoryId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return Ok(ApiResponse<object>.SuccessResponse(new { message = "Updated" }));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound(ApiResponse<object>.Failure("Product not found", null, 404));
            await _repo.DeleteAsync(existing);
            return Ok(ApiResponse<object>.SuccessResponse(new { message = "Deleted" }));
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var products = await _repo.GetAllAsync(1, 100, null, categoryId);
            return Ok(ApiResponse<PaginatedResult<Product>>.SuccessResponse(products));
        }
    }
}