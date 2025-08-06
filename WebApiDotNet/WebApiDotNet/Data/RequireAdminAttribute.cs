using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebApiDotNet.Data
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check for role claims (both "role" and "Role" claims)
            var roleClaim = user.FindFirst("role")?.Value ?? 
                           user.FindFirst("Role")?.Value ?? 
                           user.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleClaim))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Map roles from JWT to ASP.NET Core roles
            var mappedRole = roleClaim.ToUpper() switch
            {
                "ADMIN" => "Admin",
                "USER" => "User",
                "TECHNICIAN" => "Technician",
                _ => roleClaim
            };

            // Check if user has Admin role
            if (mappedRole != "Admin")
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
} 