using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllAsync();
        Task<List<Service>> GetDeletedAsync();
        Task<Service> GetByIdAsync(string id);
        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(string id, Service service);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsDeletedAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIconAsync(string icon);
    }
} 