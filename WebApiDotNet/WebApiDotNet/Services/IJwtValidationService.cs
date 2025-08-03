using System.Security.Claims;

namespace WebApiDotNet.Services
{
    public interface IJwtValidationService
    {
        ClaimsPrincipal? ValidateToken(string token);
        string? GetRoleFromToken(string token);
    }
}