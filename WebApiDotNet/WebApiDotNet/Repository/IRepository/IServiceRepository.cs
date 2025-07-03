using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(string id);
        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(string id, Service service);
        Task<bool> DeleteAsync(string id);
    }
} 