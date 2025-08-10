using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Services;
using Microsoft.Extensions.Options;
using WebApiDotNet.Data;
using WebApiDotNet.Models;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepo;
        private readonly IRoleService _roleService;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public AuthController(
            IConfiguration configuration,
            IUserRepository userRepo,
            IRoleService roleService,
            IOptions<JwtSettings> jwtOptions,
            IEmailService emailService)
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _roleService = roleService;
            _jwtSettings = jwtOptions.Value;
            _emailService = emailService;
        }

        public sealed class LoginRequest
        {
            public string? Email { get; set; }
            public string Password { get; set; } = string.Empty;
        }

        public sealed class LoginResponse
        {
            public string AccessToken { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
            public string? Role { get; set; }
            public string? UserId { get; set; }
            public string? Email { get; set; }
            public string? FullName { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var email = (req.Email ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Email and password are required" });

            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // Block login for locked or deleted users
            if (user.DeletedAt.HasValue || user.Status == UserStatus.BLOCKED)
            {
                return Forbid();
            }

            var pwdHash = user.PasswordHash ?? string.Empty;
            var isBcrypt = pwdHash.StartsWith("$2a$") || pwdHash.StartsWith("$2b$") || pwdHash.StartsWith("$2y$");
            var verified = false;
            if (isBcrypt)
            {
                verified = BCrypt.Net.BCrypt.Verify(req.Password, pwdHash);
                    }
                    else
                    {
                // Plaintext fallback + upgrade
                if (!string.IsNullOrEmpty(pwdHash) && string.Equals(pwdHash, req.Password))
                {
                    verified = true;
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
                    await _userRepo.UpdateAsync(user);
                }
            }

            if (!verified) return Unauthorized(new { message = "Invalid email or password" });

            string roleName = "User";
            if (!string.IsNullOrEmpty(user.Role))
            {
                var roleDto = await _roleService.GetByIdAsync(user.Role);
                roleName = roleDto?.Name ?? roleName;
            }

            // Ensure role claim is uppercase to match [Authorize(Roles="ADMIN")]
            var roleUpper = (roleName ?? string.Empty).ToUpperInvariant();
            var token = GenerateJwt(user.Id, user.Email, user.FullName, roleUpper);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Set HttpOnly cookie for access token only (no refresh token)
            Response.Cookies.Append(_jwtSettings.AccessTokenCookieName, token, BuildCookieOptions(jwt.ValidTo));

            return Ok(new LoginResponse
            {
                AccessToken = token,
                ExpiresAt = jwt.ValidTo,
                Role = roleName,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Clear access token cookie
            Response.Cookies.Delete(_jwtSettings.AccessTokenCookieName, BuildCookieOptions(DateTime.UtcNow.AddDays(-1)));

            return Ok(new { message = "Logged out" });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid token" });

            var users = await _userRepo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound(new { message = "User not found" });

            string roleName = "User";
            if (!string.IsNullOrEmpty(user.Role))
            {
                var roleDto = await _roleService.GetByIdAsync(user.Role);
                roleName = roleDto?.Name ?? roleName;
                }

                return Ok(new
                {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                role = roleName
            });
        }

        private string GenerateJwt(string userId, string? email, string? fullName, string role)
        {
            var key = _jwtSettings.Key ?? throw new InvalidOperationException("Jwt:Key is missing");
            var issuer = _jwtSettings.Issuer;
            var audience = _jwtSettings.Audience;
            var minutes = _jwtSettings.AccessTokenMinutes;

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new("userId", userId),
                new("email", email ?? string.Empty),
                new("fullName", fullName ?? string.Empty),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes > 0 ? minutes : 60),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Removed refresh token flow for simplicity

        public sealed class ChangePasswordRequest
        {
            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CurrentPassword) || string.IsNullOrWhiteSpace(req.NewPassword))
                return BadRequest(new { message = "Current password and new password are required" });

            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var users = await _userRepo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound(new { message = "User not found" });

            var pwdHash = user.PasswordHash ?? string.Empty;
            var isBcrypt = pwdHash.StartsWith("$2a$") || pwdHash.StartsWith("$2b$") || pwdHash.StartsWith("$2y$");
            var verified = false;
            if (isBcrypt)
            {
                verified = BCrypt.Net.BCrypt.Verify(req.CurrentPassword, pwdHash);
            }
            else if (!string.IsNullOrEmpty(pwdHash) && string.Equals(pwdHash, req.CurrentPassword))
            {
                verified = true;
            }

            if (!verified) return Unauthorized(new { message = "Current password is incorrect" });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            await _userRepo.UpdateAsync(user);
            return Ok(new { message = "Password changed" });
        }

        public sealed class ForgotPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var email = (req.Email ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(email)) return BadRequest(new { message = "Email is required" });

            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
            {
                // Fallback case-insensitive
                var all = await _userRepo.GetAllAsync();
                user = all.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    // Avoid user enumeration: return 200 generic
                    return Ok(new { message = "If that email exists, a reset instruction has been sent" });
                }
            }

            // Stub: mark a verification code and expiry (email sending omitted)
            user.VerificationCode = Guid.NewGuid().ToString("N");
            user.VerificationCodeExpires = DateTime.UtcNow.AddMinutes(60); // extend to 60 minutes
            await _userRepo.UpdateAsync(user);

            // Send email with reset link
            var frontendUrl = _configuration["Email:FrontendResetUrl"] ?? "https://front-d91nxzzw6-kds-projects-ce66334c.vercel.app/reset-password";
            var resetLink = $"{frontendUrl}?code={user.VerificationCode}&email={Uri.EscapeDataString(email)}";
            var html = $"<p>We received a request to reset your password.</p><p><a href='{resetLink}'>Click here to reset</a> (valid for 15 minutes).</p>";
            try
            {
                await _emailService.SendAsync(email, "Reset your password", html);
            }
            catch { /* ignore sending errors, still respond 200 */ }

            return Ok(new { message = "If that email exists, a reset instruction has been sent" });
        }

        public sealed class ResetPasswordRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var email = (req.Email ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Code) || string.IsNullOrWhiteSpace(req.NewPassword))
            {
                return BadRequest(new { message = "Email, code and new password are required" });
            }

            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null)
            {
                // Fallback case-insensitive
                var all = await _userRepo.GetAllAsync();
                user = all.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid reset request" });
                }
            }

            if (string.IsNullOrEmpty(user.VerificationCode) || !string.Equals(user.VerificationCode, req.Code, StringComparison.OrdinalIgnoreCase) ||
                (user.VerificationCodeExpires.HasValue && user.VerificationCodeExpires.Value < DateTime.UtcNow))
            {
                return BadRequest(new { message = "Reset code is invalid or expired" });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.VerificationCode = null;
            user.VerificationCodeExpires = null;
            await _userRepo.UpdateAsync(user);

            return Ok(new { message = "Password has been reset" });
        }

        private CookieOptions BuildCookieOptions(DateTime? expires = null)
        {
            var isHttps = HttpContext.Request.IsHttps || string.Equals(HttpContext.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.OrdinalIgnoreCase);
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps,
                SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = expires
            };
        }
    }
}

