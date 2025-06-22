using WebApiDotNet.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDotNet.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllAsync();
    }
} 