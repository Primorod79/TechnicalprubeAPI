using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Helpers;
using EcommerceAPI.Core.Interfaces;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IImageRepository _repo;

        public ImagesController(IWebHostEnvironment env, IImageRepository repo)
        {
            _env = env;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _repo.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<Image>>.SuccessResponse(images));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var image = await _repo.GetByIdAsync(id);
            if (image == null) return NotFound(ApiResponse<object>.Failure("Image not found", null, 404));
            return Ok(ApiResponse<Image>.SuccessResponse(image));
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var images = await _repo.GetByProductIdAsync(productId);
            return Ok(ApiResponse<IEnumerable<Image>>.SuccessResponse(images));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int? productId)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<object>.Failure("No file uploaded", null, 400));

            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/images/{fileName}";

            // Save to database
            var image = new Image
            {
                FileName = fileName,
                OriginalFileName = file.FileName,
                Url = url,
                ContentType = file.ContentType,
                SizeInBytes = file.Length,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repo.AddAsync(image);
            return Ok(ApiResponse<Image>.SuccessResponse(created));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _repo.GetByIdAsync(id);
            if (image == null) return NotFound(ApiResponse<object>.Failure("Image not found", null, 404));

            // Delete physical file
            var filePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", image.FileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Delete from database
            await _repo.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(new { message = "Image deleted successfully" }));
        }
    }
}