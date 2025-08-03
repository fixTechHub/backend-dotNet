using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Services;
using WebApiDotNet.Attributes;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtValidationService _jwtValidationService;

        public AuthController(IJwtValidationService jwtValidationService)
        {
            _jwtValidationService = jwtValidationService;
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
    }
} 