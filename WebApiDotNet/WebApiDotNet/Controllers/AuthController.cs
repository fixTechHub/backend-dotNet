using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiDotNet.Data;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;
using WebApiDotNet.Models;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AuthController(IConfiguration configuration, IUserService userService, IRoleService roleService)
        {
            _configuration = configuration;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet("decode-token")]
        [AllowAnonymous]
        public IActionResult DecodeToken([FromQuery] string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var claims = jwtToken.Claims.Select(c => new { type = c.Type, value = c.Value }).ToList();

                return Ok(new
                {
                    message = "Token decoded successfully",
                    claims = claims,
                    header = jwtToken.Header,
                    payload = jwtToken.Payload
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid token", error = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value;
                var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(ClaimTypes.Role)?.Value;
                var userName = User.FindFirst("fullName")?.Value ?? User.FindFirst("name")?.Value;
                var userPhone = User.FindFirst("phone")?.Value;

                // Debug: Log tất cả claims
                Console.WriteLine("=== GetCurrentUser Debug ===");
                Console.WriteLine($"User ID: {userId}");
                Console.WriteLine($"User Email: {userEmail}");
                Console.WriteLine($"User Role: {userRole}");
                Console.WriteLine($"User Name: {userName}");
                Console.WriteLine("All Claims:");
                foreach (var claim in User.Claims)
                {
                    Console.WriteLine($"  {claim.Type}: {claim.Value}");
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Token không hợp lệ" });
                }

                return Ok(new
                {
                    message = "Token hợp lệ",
                    user = new
                    {
                        id = userId,
                        email = userEmail,
                        name = userName,
                        phone = userPhone,
                        role = userRole
                    },
                    claims = User.Claims.Select(c => new { type = c.Type, value = c.Value }).ToList()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetCurrentUser error: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("validate")]
        [AllowAnonymous]
        public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string? authorization, [FromQuery] string? token)
        {
            try
            {
                // Xử lý token format - ưu tiên query parameter, sau đó là header
                string? jwtToken = null;
                
                // 1. Kiểm tra query parameter trước
                if (!string.IsNullOrEmpty(token))
                {
                    jwtToken = token;
                }
                // 2. Kiểm tra Authorization header
                else if (!string.IsNullOrEmpty(authorization))
                {
                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        jwtToken = authorization.Substring("Bearer ".Length);
                    }
                    else
                    {
                        // Nếu không có "Bearer " prefix, coi như toàn bộ là token
                        jwtToken = authorization;
                    }
                }
                // 3. Kiểm tra cookie (nếu có)
                else
                {
                    var cookieToken = Request.Cookies["token"] ?? Request.Cookies["jwt_token"];
                    if (!string.IsNullOrEmpty(cookieToken))
                    {
                        jwtToken = cookieToken;
                    }
                }

                if (string.IsNullOrEmpty(jwtToken))
                {
                    return Unauthorized(new { message = "Token không được cung cấp" });
                }

                // Validate token manually
                var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
                if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
                {
                    return StatusCode(500, new { message = "JWT settings not configured" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role
                };

                try
                {
                    var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
                    
                    // Extract claims từ validated token
                    var userId = principal.FindFirst("userId")?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var userEmail = principal.FindFirst("email")?.Value ?? principal.FindFirst(ClaimTypes.Email)?.Value;
                    var userRole = principal.FindFirst("role")?.Value ?? principal.FindFirst(ClaimTypes.Role)?.Value;
                    var userName = principal.FindFirst("fullName")?.Value ?? principal.FindFirst("name")?.Value;

                    // Debug: Log tất cả claims
                    Console.WriteLine("=== ValidateToken Debug ===");
                    Console.WriteLine($"User ID: {userId}");
                    Console.WriteLine($"User Email: {userEmail}");
                    Console.WriteLine($"User Role: {userRole}");
                    Console.WriteLine($"User Name: {userName}");
                    Console.WriteLine("All Claims:");
                    foreach (var claim in principal.Claims)
                    {
                        Console.WriteLine($"  {claim.Type}: {claim.Value}");
                    }

                    // Map role từ token sang role của ASP.NET Core
                    var mappedRole = userRole?.ToUpper() switch
                    {
                        "ADMIN" => "Admin",
                        "USER" => "User", 
                        "TECHNICIAN" => "Technician",
                        _ => userRole
                    };

                    return Ok(new
                    {
                        message = "Token hợp lệ",
                        isValid = true,
                        user = new
                        {
                            id = userId,
                            email = userEmail,
                            name = userName,
                            role = userRole,
                            mappedRole = mappedRole
                        },
                        isInRoleAdmin = userRole?.ToUpper() == "ADMIN",
                        isInRoleUser = userRole?.ToUpper() == "USER",
                        isInRoleTechnician = userRole?.ToUpper() == "TECHNICIAN"
                    });
                }
                catch (Exception tokenEx)
                {
                    return Unauthorized(new { message = "Token không hợp lệ", error = tokenEx.Message });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ValidateToken error: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("test-cookie")]
        [AllowAnonymous]
        public IActionResult TestCookie()
        {
            var cookies = Request.Cookies;
            var cookieList = new List<object>();
            
            foreach (var cookie in cookies)
            {
                cookieList.Add(new { name = cookie.Key, value = cookie.Value });
            }
            
            return Ok(new
            {
                message = "Cookie test",
                cookies = cookieList,
                hasToken = cookies.ContainsKey("token"),
                hasJwtToken = cookies.ContainsKey("jwt_token")
            });
        }
    }
}