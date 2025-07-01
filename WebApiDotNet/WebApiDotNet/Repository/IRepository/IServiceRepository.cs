using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(string id);
    }
} 