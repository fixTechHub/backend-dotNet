using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Services;
using WebApiDotNet.Attributes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtValidationService _jwtValidationService;
        private readonly IConfiguration _configuration;

        public AuthController(IJwtValidationService jwtValidationService, IConfiguration configuration)
        {
            _jwtValidationService = jwtValidationService;
            _configuration = configuration;
        }

        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            try
            {
                // Lấy token từ header
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "No token provided" });
                }

                var token = authHeader.Substring("Bearer ".Length);
                
                // Validate token và lấy role
                var role = _jwtValidationService.GetRoleFromToken(token);
                
                if (string.IsNullOrEmpty(role))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                // Trả về thông tin user từ token
                return Ok(new { 
                    message = "Token is valid",
                    role = role,
                    authenticated = true
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Token validation failed", error = ex.Message });
            }
        }

        [HttpGet("test-token")]
        public IActionResult GetTestToken()
        {
            try
            {
                var secretKey = _configuration["JwtSettings:SecretKey"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("userId", "admin-user"),
                    new Claim("email", "admin@fixtech.com"),
                    new Claim("role", "ADMIN"),
                    new Claim("fullName", "Admin User")
                };

                var token = new JwtSecurityToken(
                    issuer: "fixtech",
                    audience: "admin",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { 
                    token = tokenString,
                    message = "Test token created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to create test token", error = ex.Message });
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { 
                message = "Backend is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
} 