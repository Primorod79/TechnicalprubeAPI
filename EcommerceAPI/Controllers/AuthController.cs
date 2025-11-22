using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EcommerceAPI.Data;
using EcommerceAPI.DTOs.Auth;
using EcommerceAPI.Helpers;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest(ApiResponse<object>.Failure("Email already in use", new { email = new[] { "Email already in use" } }, 400));
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest(ApiResponse<object>.Failure("Username already in use", new { username = new[] { "Username already in use" } }, 400));

            var user = new User
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = Core.Enums.Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.SuccessResponse(new { message = "Registered" }));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(ApiResponse<object>.Failure("Invalid credentials", null, 401));

            var token = Helpers.JwtHelper.GenerateToken(user, _config);
            return Ok(ApiResponse<object>.SuccessResponse(new { token }));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) return NotFound(ApiResponse<object>.Failure("User not found", null, 404));
            return Ok(ApiResponse<object>.SuccessResponse(new { user.Id, user.Email, user.Username, user.Role }));
        }
    }
}