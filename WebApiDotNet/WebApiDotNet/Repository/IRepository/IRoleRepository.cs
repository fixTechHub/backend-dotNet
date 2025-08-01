using WebApiDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(string id);
    }
} 