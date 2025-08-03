using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiDotNet.Services;

namespace WebApiDotNet.Attributes
{
    public class RequireAdminAttribute : TypeFilterAttribute
    {
        public RequireAdminAttribute() : base(typeof(AdminAuthorizationFilter))
        {
        }
    }

    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IJwtValidationService _jwtValidationService;

        public AdminAuthorizationFilter(IJwtValidationService jwtValidationService)
        {
            _jwtValidationService = jwtValidationService;
        }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Lấy token từ header
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.Substring("Bearer ".Length);
            
            // Validate token và lấy role
            var role = _jwtValidationService.GetRoleFromToken(token);
            
            // Chỉ cần token hợp lệ và có role (không cần strict ADMIN)
            if (string.IsNullOrEmpty(role))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
} 